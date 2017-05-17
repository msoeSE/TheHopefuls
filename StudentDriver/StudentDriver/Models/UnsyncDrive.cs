using System;
using SQLite.Net.Attributes;

namespace StudentDriver.Models
{
	[Table("UnsyncDrives")]
	public class UnsyncDrive
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }
	}
}
