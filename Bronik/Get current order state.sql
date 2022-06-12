select 
	max(oh.id) as 'ID',
	oh.full_name as 'ФИО',
	oh.quantity as 'Человек',
	oh.'from' as 'С',
	oh.phone as 'Телефон',
	oh.state as 'Статус'
	from 'order' o join order_history oh on o.id=oh.order_id where o.id=1