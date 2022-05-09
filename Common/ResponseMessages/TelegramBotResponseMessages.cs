using Freshness.Common.Constants;

namespace Freshness.Common.ResponseMessages
{
    public static class TelegramBotResponseMessages
    {
        public const string InputPhone = "Введіть номер телефону БЕЗ коду (+38):";

        public const string InputPassword = "Введіть будь-ласка пароль:";

        public const string UnknownCommand = "Не зрозуміла команда, спробуйте ще раз!";

        public const string AlreadyAuthorized = "Ви вже авторизовані у системі. Для вас доступна тільки команда для виходу з системи - " + TelegramBotCommand.Delete + " !";
        
        public const string AlreadyAuthorizedFromDifferentDevice = "Користувач з цим номером телефону вже авторизований у системі на іншому пристрої. Вийдіть з облікового запису або зверніться до адміністратора.";

        public const string UserDoesNotExist = "На жаль користувач не зареєстрований в системі!";

        public const string WrongPassword = "Невірний пароль! Спробуйте ще раз, або введіть команду " + TelegramBotCommand.Cancel + ", щоб скасувати авторизацію.";

        public const string SuccessfullySignedIn = "Вітаю! Ви успішно авторизувались у системі!";

        public const string SuccessfullySignedOut = "Ви успішно вийшли із системи!";
    }
}
