using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Reflection;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";
        public const string WRONG_MAX_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXT";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] NewFileWrongSize =
        {
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING }
        };

        static object[] NullFileData =
        {
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        static object[] WrongMaxSizeData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), new File(TIC_TOC_TOE_STRING, WRONG_MAX_SIZE_CONTENT_STRING) }
        };

        /* Тестирование записи файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void WriteTest(File file) 
        {
            //Arrange
            storage.DeleteAllFiles(); //* ИСПРАВИЛ
            //Act and Assert
            Assert.True(storage.Write(file));
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [Test, TestCaseSource(nameof(NewExceptionFileData))]
        public void WriteExceptionTest(File file) {
            //Arrange
            bool isException = false;
            //Act
            try
            {
                storage.Write(file);
                Assert.False(storage.Write(file));
                storage.DeleteAllFiles();
            } 
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            //Assert
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void IsExistsTest(File file) {
            //Arrange
            storage.DeleteAllFiles();
            String name = file.GetFilename();
            Assert.False(storage.IsExists(name));
            //Act
            try {
                storage.Write(file);
            } catch (FileNameAlreadyExistsException e) {
                Console.WriteLine(String.Format("Exception {0} in method {1}", e.GetBaseException(), MethodBase.GetCurrentMethod().Name));
            }
            //Assert
            Assert.True(storage.IsExists(name));
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [Test, TestCaseSource(nameof(FilesForDeleteData))]
        public void DeleteTest(File file, String fileName) {
            //Arrange
            storage.Write(file);
            //Act and Assert
            Assert.True(storage.Delete(fileName));
        }

        /* Тестирование получения файлов */
        [Test]
        public void GetFilesTest()
        {
            //Act and Assert
            foreach (File el in storage.GetFiles()) 
            {
                Assert.NotNull(el);
            }
        }

        // Почти эталонный
        /* Тестирование получения файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void GetFileTest(File expectedFile) 
        {
            //Act and Assert
            storage.Write(expectedFile);

            File actualfile = storage.GetFile(expectedFile.GetFilename());
            bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());

            //Assert.IsFalse(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize())); НЕПРАВИЛЬНО
            Assert.True(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize())); //ИСПРАВЛЕНО
        }

        //Мои тесты
        
        //Тестирование удаления всех файлов
        [Test, TestCaseSource(nameof(FilesForDeleteData))]
        public void DeleteAllFilesTest(File file, String fileName)
        {
            //Arrange
            storage.Write(file);
            //Act and Assert
            Assert.True(storage.DeleteAllFiles());
        }

        //Тестирование на запись слишком большого файла
        [Test, TestCaseSource(nameof(NewFileWrongSize))]
        public void WriteWrongSizeFile(File file)
        {
            //Arrange
            storage.DeleteAllFiles();
            //Act and Assert
            Assert.False(storage.Write(file));
        }

        /* Тестирование на запись пустого значения */
        [Test, TestCaseSource(nameof(NullFileData))]
        public void WriteNullFileTest(File file, String fileName)
        {
            //Arrange
            bool Exception = false;
            //Act
            try
            {
                Assert.False(storage.Write(file));
            }
            catch (NullReferenceException)
            {
                Exception = true;
            }
            //Assert
            Assert.True(Exception);
        }

        /* Тестирование на удаление несуществующего файла */
        [Test, TestCaseSource(nameof(NullFileData))]
        public void DeleteNullFileTest(File file, String fileName)
        {
            //Act and Assert
            Assert.False(storage.Delete(fileName));
        }

        /* Тестирование на поиск несуществующего файла */
        [Test, TestCaseSource(nameof(NullFileData))]
        public void IsNotExistTest(File file, String fileName)
        {
            //Act and Assert
            Assert.False(storage.IsExists(fileName));
        }

        /* Тестирование на поиск несуществующего файла */
        [Test, TestCaseSource(nameof(NullFileData))]
        public void GetNullFileTest(File file, String fileName)
        {
            //Act and Assert
            Assert.IsNull(storage.GetFile(fileName));
        }

        //Тестирование на заполнение размера
        [Test, TestCaseSource(nameof(WrongMaxSizeData))]
        public void WrongMaxSizeTest(File file1, File file2)
        {
            //Act and Assert
            Assert.True(storage.Write(file1)); //Заполнение
            Assert.False(storage.Write(file2)); //С учетом заполнения места не хватит
        }
    }
}
