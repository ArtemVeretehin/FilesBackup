using NLog;
internal class Backuper
{
    public string? DestinationFolder { get; set; }
    public List<string>? StartFolders { get; set; }
    public List<DirectoryInfo>? StartDirectoryInfos = new List<DirectoryInfo>();


    //Функция для заполнения листа DirectoryInfo на основе полученных из конфигурации исходных директорий
    public void DirectoryInfoList_Fill()
    {
        //Если получены данные об исходных директориях
        if (StartFolders != null)
        {
            //Для каждой исходной директории в JSON-конфиге
            foreach (string StartDirectory in StartFolders)
            {
                //Добавляем исходную директорию в виде объекта DirectoryInfo в лист DirectoryInfo-объектов
                StartDirectoryInfos?.Add(new DirectoryInfo(StartDirectory));
            }
        }
    }


    //Функция резервного копирования
    public void Backup(Logger logger)
    {
        //Формируем имя директории для текущего сеанса резервного копирования (целевая директория + временная метка)
        string CurrentSessionDirectory = System.IO.Path.Combine(DestinationFolder, System.Convert.ToString(DateTime.Now).Replace(":", "_"));

        //Создаем директорию для текущего сеанса резервного копирования
        Directory.CreateDirectory(CurrentSessionDirectory);

        logger.Info("Создана директория для текущего сеанса резервного копирования");

        //Для каждой исходной директории
        foreach (DirectoryInfo StartDirectoryInfo in StartDirectoryInfos)
        {
            try
            {
                //Получаем все файлы в исходной директории в виде массива FileInfo
                FileInfo[] StartDirectory_Files = StartDirectoryInfo.GetFiles();

                //Для каждого файла в исходной директории формируем путь резервного копирования и производим бэкап
                foreach (FileInfo file in StartDirectory_Files)
                {
                    string FullBackupPath = System.IO.Path.Combine(CurrentSessionDirectory, file.Name);
                    file.CopyTo(FullBackupPath, true);
                    logger.Debug($"Копирование файла {file.Name} выполнено успешно");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
    }

}


