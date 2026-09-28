function Card({ name, phone, email, city, group }) {
  const items = [
    { label: 'Телефон', value: phone, href: `tel:${phone.replace(/\s/g, '')}` },
    { label: 'Електронна адреса', value: email, href: `mailto:${email}` },
    { label: 'Місто', value: city },
    { label: 'Група', value: group },
  ]

  return (
    <article className="card">
      <div className="card__avatar">{name.charAt(0)}</div>
      <h2 className="card__title">{name}</h2>
      <ul className="card__items">
        {items.map((item) => (
          <li key={item.label} className="card__item">
            <span className="card__label">{item.label}</span>
            {item.href ? (
              <a href={item.href}>{item.value}</a>
            ) : (
              <span>{item.value}</span>
            )}
          </li>
        ))}
      </ul>
    </article>
  )
}

export default Card
