using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.API.DTOs;
public record PaginationQuery
{
	[Range(1, int.MaxValue)]
	public int PageNumber { get; set; } = 1;

	[Range(1, int.MaxValue)]
	public int PageSize { get; set; } = 5;
}
