internal class Backuper
{
    public string? DestinationFolder { get; set; }
    public List<string>? StartFolders { get; set; }
    public List<DirectoryInfo>? StartDirectoryInfos = new List<DirectoryInfo>();


    //Функция для заполнения листа DirectoryInfo на основе полученных из конфигурации исходных директорий
    public void DirectoryInfoList_Fill()
    {
        if (StartFolders != null)
        {
            foreach (string StartDirectory in StartFolders)
            {
                //Добавляем исходную директорию в виде объекта DirectoryInfo в лист DirectoryInfo-объектов
                StartDirectoryInfos?.Add(new DirectoryInfo(StartDirectory));
            }
        }
    }


    //Функция резервного копирования
    public void Backup()
    {
        //Проверяем существует ли целевая директория
        if (Directory.Exists(DestinationFolder))
        {
            if (StartDirectoryInfos != null)
            {
                //Для каждой исходной директории
                foreach (DirectoryInfo StartDirectoryInfo in StartDirectoryInfos)
                {
                    //Формируем имя директории для текущего сеанса резервного копирования
                    string CurrentSessionDirectory = System.IO.Path.Combine(DestinationFolder, System.Convert.ToString(DateTime.Now).Replace(":", "_"));

                    //Создаем директорию
                    Directory.CreateDirectory(CurrentSessionDirectory);

                    //Получаем все файлы в исходной директории в виде массива FileInfo
                    FileInfo[] StartDirectory_Files = StartDirectoryInfo.GetFiles();

                    //Для каждого файла в исходной директории формируем путь резервного копирования и производим бэкап
                    foreach (FileInfo file in StartDirectory_Files)
                    {
                        string FullBackupPath = System.IO.Path.Combine(CurrentSessionDirectory, file.Name);
                        file.CopyTo(FullBackupPath, true);
                    }
                }
            }
        }
    }
}


