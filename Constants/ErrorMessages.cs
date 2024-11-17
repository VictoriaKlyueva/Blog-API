namespace BackendLaboratory.Constants
{
    public static class ErrorMessages
    {
        public const string InvalidEmail = "Неверный формат почты.";
        public const string InvalidBirthDate = "Неверная дата. Дата должна быть позднее 1915 года и не позднее текущей даты.";
        public const string InvalidFullName = "Неверная имя пользователя. Имя пользователя должно быть записано в формате <Фамилия Имя Отчество>, отчество может отсутствовать.";
        public const string WeakPassword = "Слабый пароль. Сильный пароль должен иметь как минимум одну заглавную букву, одну строчную букву, одну цифру, один специальный символ.";
        public const string InvalidPhoneNumber = "Неверный формат номера телефона. Телефон должен соответствовать маске +7 (xxx) xxx-xx-xx.";
        public const string IncorrectId = "Некорректный ID: не удалось извлечь или преобразовать id из токена.";
        public const string ProfileNotFound = "Пользователь не найден.";
    }
}
