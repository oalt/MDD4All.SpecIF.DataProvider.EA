using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.DataProvider.EA.Converters;
using MDD4All.SpecIF.DataProvider.File;
using System;
using System.IO;
using System.Reflection;
using EAAPI = EA;

namespace MDD4All.SoecIF.DataProvider.EA.Test
{
    public class TestEaSpecIfConverter
	{

		public void StartTest()
		{

			string progId = "EA.Repository";
			Type type = Type.GetTypeFromProgID(progId);
			EAAPI.Repository repository = Activator.CreateInstance(type) as EAAPI.Repository;

			try
			{
				string fileName = "TestModel1.eap";

				bool openResult = repository.OpenFile(AssemblyDirectory + "/TestData/" + fileName);

				// root package guid: {45A143E0-D43A-4f51-ACA6-FF695EEE3256}

				if (openResult)
				{
					Console.WriteLine("Model open");

					//EAAPI.Package rootModelPackage = repository.GetPackageByGuid("{45A143E0-D43A-4f51-ACA6-FF695EEE3256}");

					EAAPI.Diagram diagram = repository.GetDiagramByGuid("{BF30DD09-E13E-451b-B210-786F93A74936}") as EAAPI.Diagram;

					SpecIF.DataModels.SpecIF specIF;

					EaUmlToSpecIfConverter converter;

					ISpecIfMetadataReader metadataReader = new SpecIfFileMetadataReader<ExtendedSpecIF>("c:\\specifClasses");

					converter = new EaUmlToSpecIfConverter(repository, metadataReader);

					Resource resource = converter.ConvertDiagram(diagram);


					//specIF = converter.ConvertModelToSpecIF(rootModelPackage);
						

					//SpecIfFileReaderWriter.SaveSpecIfToFile(specIF, @"D:\speciftest\TestModel1.specif");

					Console.WriteLine("Finished");
				}
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception);
			}
			finally
			{
				//Console.ReadLine();
				repository.Exit();
			}

		}

		

		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
	}
}
