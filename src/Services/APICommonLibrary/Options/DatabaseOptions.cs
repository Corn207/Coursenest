using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICommonLibrary.Options;
public class DatabaseOptions
{
	public const string SectionName = "Database";

	public bool Overwrite { get; set; } = false;

	public bool Create { get; set; } = false;

	public bool Seed { get; set; } = false;
}
