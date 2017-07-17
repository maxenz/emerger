namespace Emerger.DomainModel
{
	/// <summary>
	/// Clase para representar los datos de un filtro usado en el frontend
	/// </summary>
	public class Filter
	{
		#region Properties

		public int Id { get; set; }

		public string Description { get; set; }

		#endregion

		#region Constructors

		public Filter() { }

		public Filter(int id, string description)
		{
			this.Id = id;
			this.Description = description;
		}

		#endregion
	}
}
