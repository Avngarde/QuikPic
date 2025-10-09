using System;
using Microsoft.EntityFrameworkCore;
using QuikPic.Web.Models;

namespace QuikPic.Web;

public class QuikPicContext : DbContext
{
    public QuikPicContext(DbContextOptions<QuikPicContext> options) : base(options) { }

    public DbSet<Preset> Presets { get; set; }
}
