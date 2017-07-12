using ShamanExpressDLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ShamanRabbitService
{
	public class DatabaseConnection
	{

		/// <summary>
		/// Instancia del Logger
		/// </summary>
		private static DatabaseConnection _instance;

		/// <summary>
		/// Para multithreading
		/// </summary>
		private static object syncLock = new object();


		#region Constructor

		protected DatabaseConnection()
		{

		}

		#endregion

		public static DatabaseConnection GetInstance()
		{
			if (_instance == null)
			{
				lock (syncLock)
				{
					if (_instance == null)
					{
						_instance = new DatabaseConnection();
					}
				}
			}

			return _instance;
		}


		public bool Connect()
		{
			StartUp init = new StartUp();

			if (init.GetValoresHardkey(false))
			{

				/*
                '-------> Revisar antes el registro...

                '-------> HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Shaman\Express    '-> 64 bits
                '-------> HKEY_LOCAL_MACHINE\SOFTWARE\Shaman\Express    '-> 32 bits
                '-------> que estén los valores:
                '-------> cnnDataSource (SERVER\INSTANCIASQL)   200.49.156.125,9898\SQLEXPRESS
                '-------> cnnCatalog (database: Shaman)         Shaman
                '-------> cnnUser (user sql)                    dbaadmin
                '-------> cnnPassword (password sql)            yeike
                '-------> sysProductos (poner en 1)             1
                */

				if (init.GetVariablesConexion(true, modDeclares.keyMode.keyAll))
				{

					if (init.AbrirConexion(modDeclares.cnnDefault))
					{
						modFechas.InitDateVars();
						modNumeros.InitSepDecimal();
						modDeclares.shamanConfig = new conConfiguracion();
						modDeclares.shamanConfig.UpConfig();
						modDeclares.shamanSession = new conUsuarios();
						//Logger.GetInstance().AddLog(true, "setConexionDB", "Conectado a Database Shaman");
						return true;
					}
					else
					{
						//Logger.GetInstance().AddLog(false, "setConexionDB", "No se pudo conectar a base de datos Shaman - " + init.MyLastExec.ErrorDescription);
					}
				}
				else
				{
					//Logger.GetInstance().AddLog(false, "setConexionDB", "No se pudieron recuperar las variables de conexión - " + init.MyLastExec.ErrorDescription);
				}
			}
			else
			{
				//Logger.GetInstance().AddLog(false, "setConexionDB", "No se encuentran los valores HKey - " + init.MyLastExec.ErrorDescription);
			}

			return false;

		}

	}
}
