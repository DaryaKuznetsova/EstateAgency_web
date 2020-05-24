namespace EstateAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("EstateObject")]
    public partial class EstateObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EstateObject()
        {
            PictureObjectLinks = new HashSet<PictureObjectLink>();
            Requests = new HashSet<Request>();
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public string Address { get; set; }

        
        public string Description { get; set; }
        [Required]
        public double Area { get; set; }

        public int? Rooms { get; set; }

        public string LandDescription { get; set; }

        public double? LandArea { get; set; }
        [Required]
        public int RealtyTypeId { get; set; }
        [Required]
        public int TradeTypeId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int OwnerId { get; set; }

        public virtual District District { get; set; }

        public virtual Owner Owner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PictureObjectLink> PictureObjectLinks { get; set; }

        public virtual RealtyType RealtyType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }

        public virtual Status Status { get; set; }

        public virtual TradeType TradeType { get; set; }
    }
}
