using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vegallomas.Models
{
    public class Vendeg
    {
        [Key]
        public int vendegID { get; set; }

        [Required]
        [Display(Name="Neved")]
        public string nev { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name="Üzenet")]
        public string uzenet { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime datum { get; set; }
    }

    public class program
    {
        [Key]
        public int programID { get; set; }

        [Required]
        [Display(Name="Dátum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString="{0:yyyy. MMMM. dd. (dddd)}")]
        public DateTime datum { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name="Zenekarok")]
        public string zenakarok { get; set; }

        [Required]
        [Display(Name="Kapunyitás")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        [DataType(DataType.Time)]
        public TimeSpan nyitas { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name="Koncert kezdés")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan kezdes { get; set; }

        [Display(Name="Belépő")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int jegyar { get; set; }

        [Display(Name="Plakát (URL)")]
        public string plakat { get; set; }

        public bool elmarad { get; set; }
    }

    public class vegDB : DbContext
    {
        public DbSet<Vendeg> Vendegkonyv { get; set; }
        public DbSet<program> Koncert { get; set; }
    }
}