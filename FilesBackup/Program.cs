using System.Text.Json;
using NLog;
using NLog.Config;

LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
Logger logger = LogManager.GetLogger("Common");

Console.WriteLine("Нажмите любую кнопку для запуска резервного копирования");
Console.ReadLine();

logger.Info("Пользователь подтвердил запуск резервного копирования");


//Создаем контейнер для настроек директорий
Backuper? DirectoryConfig_Instance;


//Считываем конфиг, десеарилизуем настройки из JSON в контейнер
using (FileStream Stream = new FileStream(@"DirectoryConfig.json", FileMode.Open))
{
    logger.Info("Началась десериализация конфигурации резервного копирования из JSON");

    DirectoryConfig_Instance = await JsonSerializer.DeserializeAsync<Backuper>(Stream);

    logger.Info("Конфигурация получена");
}

logger.Info("Заполнение листа объектов DirectoryInfo на основе исходных директорий");

//Заполнение DirectoryInfo-листа у экземпляра класса Backuper
DirectoryConfig_Instance?.DirectoryInfoList_Fill();



//Проверяем существует ли целевая директория
if (Directory.Exists(DirectoryConfig_Instance?.DestinationFolder))
{
    //Если лист объектов DirectoryInfo не равен 0 (а значит в конфиге были указаны исходные папки)
    if (DirectoryConfig_Instance.StartDirectoryInfos != null)
    {
        DirectoryConfig_Instance?.Backup(logger);
        logger.Info("Резервное копирование выполнено успешно!");
    }
    else
    {
        logger.Error("В конфигурации не указано ни одной исходной папки");
    }
}
else
{
    logger.Error("Целевая папка не указана в конфигурации, либо не существует");
}