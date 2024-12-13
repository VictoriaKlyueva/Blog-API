namespace BackendLaboratory
{
        public class AppConstants
        {
            public const string Bearer = "Bearer ";

            public const string EmptyString = "";
        
            public const int TokenExpiration = 7;

            public const string Blacklisted = "blacklisted";

            public const string EmailTitle = "Ура новый пост в вашем сообществе! Бегом смотреть!";

            public const string EmailFrom = "Рассылка лучшего блога в мире";

            public const string MailBody =
                @"
                <style>
                @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap');
                body {{
                    font-family: 'Inter', sans-serif;
                    color: black;
                }}
                .title {{
                    font-size: 24px;
                    font-weight: 700;
                }}
                .subtitle {{
                    font-size: 20px;
                    font-weight: 700;
                    margin-top: 20px;
                }}
                .description {{
                    font-size: 18px;
                    font-weight: 400;
                }}
            </style>
            <h2 class='title'>Вышел новый пост в вашем сообществе: {0}</h2>
            <h3 class='subtitle'>{1}</h3>
            <p class='description'>{2}</p>";
        
        // Community titiles
        public const string MasonName = "Масонская ложа";
        public const string KanevskyName = "Следствие вели с Л. Каневским";
        public const string ItName = "IT <3";

        // Community descriptions

        public const string MasonDescription = "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами";
        public const string KanevskyDescription = "Без длинных предисловий: мужчина умер";
        public const string ItDescription = "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления";
    }
}
