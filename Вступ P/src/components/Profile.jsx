function Profile({ fullName, phone, email, city, group }) {
  return (
    <section className="profile">
      <h1 className="profile__name">{fullName}</h1>
      <dl className="profile__list">
        <div className="profile__row">
          <dt>Телефон</dt>
          <dd><a href={`tel:${phone.replace(/\s/g, '')}`}>{phone}</a></dd>
        </div>
        <div className="profile__row">
          <dt>Пошта</dt>
          <dd><a href={`mailto:${email}`}>{email}</a></dd>
        </div>
        <div className="profile__row">
          <dt>Місто</dt>
          <dd>{city}</dd>
        </div>
        <div className="profile__row">
          <dt>Група</dt>
          <dd>{group}</dd>
        </div>
      </dl>
    </section>
  )
}

export default Profile
