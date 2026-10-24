import { useState } from 'react'

interface Profile {
  name: string
  specialty: string
  isOnline: boolean
}

function ProfileEditor() {
  const [profile, setProfile] = useState<Profile>({
    name: 'Ваше Ім\'я',
    specialty: 'React Developer',
    isOnline: true,
  })

  return (
    <div className="profile-editor">
      <form className="profile-editor__form" onSubmit={(event) => event.preventDefault()}>
        <h3 className="profile-editor__heading">Налаштування</h3>

        <label className="profile-editor__field">
          Ім'я
          <input
            type="text"
            value={profile.name}
            onChange={(event) => setProfile({ ...profile, name: event.target.value })}
          />
        </label>

        <label className="profile-editor__field">
          Спеціальність
          <input
            type="text"
            value={profile.specialty}
            onChange={(event) => setProfile({ ...profile, specialty: event.target.value })}
          />
        </label>

        <label className="profile-editor__checkbox">
          <input
            type="checkbox"
            checked={profile.isOnline}
            onChange={(event) => setProfile({ ...profile, isOnline: event.target.checked })}
          />
          Онлайн
        </label>
      </form>

      <div className="profile-editor__preview">
        <div className="badge">
          <div className="badge__avatar">
            {profile.name.trim().charAt(0).toUpperCase()}
            {profile.isOnline && <span className="badge__status" />}
          </div>
          <h3 className="badge__name">{profile.name}</h3>
          <p className="badge__specialty">{profile.specialty}</p>
        </div>
      </div>
    </div>
  )
}

export default ProfileEditor
