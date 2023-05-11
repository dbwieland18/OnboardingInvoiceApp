using System.Collections.Generic;

namespace AspNetCorePostgreSQLDockerApp.Models {
  public class Invoice
  {
    public int Id { get; set; }
    public double Total { get; set; }
    public bool Paid { get; set; }
  }
}