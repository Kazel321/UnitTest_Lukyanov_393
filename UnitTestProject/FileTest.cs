using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    [TestClass]
    public class FileTest
    {

        public const string SIZE_EXCEPTION = "Wrong size";
        public const string NAME_EXCEPTION = "Wrong name";
        public const string EXTENSION_EXCEPTION = "Wrong extension";
        public const string CONTENT_EXCEPTION = "Wrong content";
        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public double lenght;
        public string extension;

        /* ПРОВАЙДЕР */
        static object[] FilesData =
        {
            new object[] {new File(FILE_PATH_STRING, CONTENT_STRING), FILE_PATH_STRING, CONTENT_STRING},
            new object[] { new File(SPACE_STRING, SPACE_STRING), SPACE_STRING, SPACE_STRING}
        };

        /* Тестируем получение размера */
        [Test, TestCaseSource(nameof(FilesData))]
        public void GetSizeTest(File newFile, String name, String content)
        {
            //Arrange
            lenght = content.Length / 2;
            //Act and Assert
            Assert.AreEqual(newFile.GetSize(), lenght, SIZE_EXCEPTION);
        }

        /* Тестируем получение имени */
        [Test, TestCaseSource(nameof(FilesData))]
        public void GetFilenameTest(File newFile, String name, String content)
        {
            //Act and Assert
            Assert.AreEqual(newFile.GetFilename(), name, NAME_EXCEPTION);
        }

        //Мои тесты

        /* Тестируем получение расширения */
        [Test, TestCaseSource(nameof(FilesData))]
        public void GetExtensionTest(File newFile, String name, String content)
        {
            //Arrange
            extension = name.Split('.')[name.Split('.').Length - 1];
            //Act and Assert
            Assert.AreEqual(newFile.GetExtension(), extension, EXTENSION_EXCEPTION);
        }

        /* Тестируем получение содержимого */
        [Test, TestCaseSource(nameof(FilesData))]
        public void GetContentTest(File newFile, String name, String content)
        {
            //Act and Assert
            Assert.AreEqual(newFile.GetContent(), content, CONTENT_EXCEPTION);
        }
    }
}
