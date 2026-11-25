import { useState } from "react";
import {
  useForm,
  useFieldArray,
  Controller,
  type SubmitHandler,
} from "react-hook-form";
import Select from "react-select";

type LanguageOption = { value: string; label: string };

type ProfileFormValues = {
  fullName: string;
  email: string;
  phones: { value: string }[]; // useFieldArray працює лише з масивом об'єктів
  languages: LanguageOption[];
};

type ProfilePayload = {
  fullName: string;
  email: string;
  phones: string[];
  languages: string[];
};

const languageOptions: LanguageOption[] = [
  { value: "uk", label: "Українська" },
  { value: "en", label: "Англійська" },
  { value: "de", label: "Німецька" },
  { value: "pl", label: "Польська" },
  { value: "fr", label: "Французька" },
  { value: "es", label: "Іспанська" },
];

export default function ProfileEditForm() {
  const [saved, setSaved] = useState<ProfilePayload | null>(null);

  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ProfileFormValues>({
    defaultValues: {
      fullName: "",
      email: "",
      phones: [{ value: "" }],
      languages: [],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "phones",
  });

  const onSubmit: SubmitHandler<ProfileFormValues> = async (data) => {
    const payload: ProfilePayload = {
      fullName: data.fullName.trim(),
      email: data.email.trim(),
      phones: data.phones.map((p) => p.value.trim()),
      languages: data.languages.map((l) => l.value),
    };
    // Тут має бути запит на бекенд
    console.log("Профіль збережено:", payload);
    setSaved(payload);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} noValidate>
      {/* 1. ПІБ */}
      <div className="field">
        <label htmlFor="fullName">ПІБ студента</label>
        <input
          id="fullName"
          placeholder="Іваненко Іван Іванович"
          className={errors.fullName ? "invalid" : ""}
          {...register("fullName", {
            required: "Вкажіть ПІБ",
            minLength: { value: 5, message: "Мінімум 5 символів" },
          })}
        />
        {errors.fullName && (
          <p className="error" role="alert">{errors.fullName.message}</p>
        )}
      </div>

      {/* 2. Електронна пошта */}
      <div className="field">
        <label htmlFor="email">Електронна пошта</label>
        <input
          id="email"
          type="email"
          placeholder="student@example.com"
          className={errors.email ? "invalid" : ""}
          {...register("email", {
            required: "Вкажіть електронну пошту",
            pattern: {
              value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
              message: "Некоректний формат пошти",
            },
          })}
        />
        {errors.email && (
          <p className="error" role="alert">{errors.email.message}</p>
        )}
      </div>

      {/* 3. Телефони (динамічний список) */}
      <div className="field">
        <label>Номери телефонів</label>
        {fields.map((field, index) => {
          const err = errors.phones?.[index]?.value;
          return (
            <div key={field.id}>
              <div className="phone-row">
                <div className="phone-input">
                  <input
                    placeholder="+380501234567"
                    className={err ? "invalid" : ""}
                    {...register(`phones.${index}.value` as const, {
                      required: "Вкажіть номер",
                      pattern: {
                        value: /^\+?[0-9\s\-()]{7,15}$/,
                        message: "Некоректний номер телефону",
                      },
                    })}
                  />
                  {err && <p className="error" role="alert">{err.message}</p>}
                </div>
                <button
                  type="button"
                  onClick={() => remove(index)}
                  disabled={fields.length === 1}
                >
                  Видалити
                </button>
              </div>
            </div>
          );
        })}
        <button type="button" onClick={() => append({ value: "" })}>
          + Додати номер
        </button>
      </div>

      {/* 4. Володіння мовами (react-select + Controller) */}
      <div className="field">
        <label htmlFor="languages">Володіння мовами</label>
        <Controller
          name="languages"
          control={control}
          rules={{
            validate: (v) => v.length > 0 || "Оберіть хоча б одну мову",
          }}
          render={({ field }) => (
            <Select<LanguageOption, true>
              {...field}
              inputId="languages"
              isMulti
              options={languageOptions}
              placeholder="Оберіть мови..."
              noOptionsMessage={() => "Нічого не знайдено"}
            />
          )}
        />
        {errors.languages && (
          <p className="error" role="alert">
            {errors.languages.message as string}
          </p>
        )}
      </div>

      <button type="submit" className="primary" disabled={isSubmitting}>
        Зберегти
      </button>

      {saved && (
        <pre className="result">{JSON.stringify(saved, null, 2)}</pre>
      )}
    </form>
  );
}
