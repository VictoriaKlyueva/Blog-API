﻿namespace BackendLaboratory.Constants
{
    public static class ErrorMessages
    {
        // Register data
        public const string InvalidEmail = "Неверный формат почты.";
        public const string InvalidBirthDate = "Неверная дата. Дата должна быть позднее 1915 года и не позднее текущей даты.";
        public const string InvalidFullName = "Неверная имя пользователя. Имя пользователя должно быть записано в формате <Фамилия Имя Отчество>, отчество может отсутствовать.";
        public const string WeakPassword = "Слабый пароль. Сильный пароль должен иметь как минимум одну заглавную букву, одну строчную букву, одну цифру, один специальный символ.";
        public const string InvalidPhoneNumber = "Неверный формат номера телефона. Телефон должен соответствовать маске +7 (xxx) xxx-xx-xx.";

        // Pagination data
        public const string InvalidSize = "На странице не может быть меньше 1-го элемента.";
        public const string InvalidPages = "Должна быть хотя бы одна страница.";
        public const string InvalidMin = "Минимум времени должен быть больше нуля.";
        public const string InvalidMax = "Максимум времени должен быть больше нуля.";
        public const string PageNotFound = "Такой страницы не существует.";

        // Other
        public const string IncorrectToken = "Некорректный или отсутствующий токен.";
        public const string IncorrectGuid = "Id не соответствует формату Guid.";
        public const string IncorrectTokenResponse = "Не удалось получить токен.";
        public const string IncorrectId = "Некорректный ID: не удалось извлечь или преобразовать id из токена.";
        public const string ProfileNotFound = "Пользователь не авторизован.";
        public const string UserIsAlreadyExcist = "Такой пользователь уже существует.";
        public const string UserIsAlreadySubstribed = "Пользователь уже подписан на эту группу.";
        public const string CommunityNotFound = "Такая группа не найдена.";
        public const string CommunitiesNotFound = "Группы не найдены.";
        public const string TagNotFound = "Тэг не существует.";
        public const string PostNotFound = "Пост не существует.";
        public const string AuthorNotFound = "Автор поста не найден";
        public const string UserIsNotSubstribed = "Пользователь не подписан на эту группу.";
        public const string ValueMustBePositive = "Значение должно быть больше 0";
        public const string UserCantMakePost = "Этот пользователь не может создавать посты в этой группе.";
        public const string PostForbidden = "Пост недоступен этому пользователю.";
        public const string CommunityPostsForbidden = "Посты этой группы недоступны этому пользователю.";
        public const string LikeIsAlreadyAdded = "Пользователь уже поставил лайк на этот пост.";
        public const string LikeHasntBeenAdded = "У пользователя нет лайка на этот пост.";
        public const string CommentNotFound = "Комментарий не найден.";
        public const string ParentCommentNotFound = "Родительский комментарий не найден.";
        public const string CommentForbidden = "Это не комментарий пользователя. Пользователь может редактировать и удалять только свои комментарии.";
        public const string CommentIsNotRoot = "Комментарий не является корневым.";
        public const string AddressNotFound = "Такой адресс не существует.";
        public const string IncorrectCommentContent = "Поле комментарий обязательно и не может состоять только из пробелов.";
        public const string IncorrectReadingTime = "Время прочтения может быть только положительным целым числом.";
        public const string IncorrectImage = "Некорректный формат фото.";
        public const string IncorrectPostTags = "У поста длолжен быть хотя бы один тэг.";

        public static string ConcreteTagNotFound(string tagId)
        {
            return $"Tag with id {tagId} not found";
        }

        public static string ConcreteCommentNotFound(string commentId)
        {
            return $"Tag with id {commentId} not found";
        }
    }
}
