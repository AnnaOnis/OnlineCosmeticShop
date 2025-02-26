export const orderStatusMap: { [key: number]: string } = {
    0: 'В ожидании',
    1: 'В обработке',
    2: 'Отправлен',
    3: 'Доставлен',
    4: 'Отменен',
    5: 'Возвращен'
  };

  export const orderPaymentMethodMap: {[key: number]: string } = {
    0: 'Картой онлайн',
    1: 'Наличными при получении'
  };

  export const orderPaymentStatusMap: { [key: number]: string } = {
    0: 'Не оплачен',
    1: 'Оплачен',
    2: 'Оплата не прошла',
    3: 'Возврат оплаты',
    4: 'Отменен'
  };

  export const orderShippingMethodMap: {[key: number] : string} = {
    0: 'Почта',
    1: 'Курьер',
    2: 'Самовывоз',
  };

  export const userRoleMap: {[key: number] : string} = {
    0: 'Администратор',
    1: 'Менеджер',
    2: 'Модератор',
  };