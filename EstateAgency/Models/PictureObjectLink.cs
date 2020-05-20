namespace EstateAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PictureObjectLink")]
    public partial class PictureObjectLink
    {
        public int Id { get; set; }

        public int EstateObjectId { get; set; }

        public int PictureId { get; set; }

        public virtual EstateObject EstateObject { get; set; }

        public virtual Picture Picture { get; set; }
    }
}
