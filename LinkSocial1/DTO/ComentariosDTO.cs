﻿using DB.Modelo;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkSocial1.DTO
{
    public class ComentariosDTO
    {

        public int idComentario { get; set; }

        [ForeignKey("usuarios")]
        public int idUsuario { get; set; }

        [ForeignKey("publicaciones")]
        public int idPublicacion { get; set; }

        public string contenidoComentario { get; set; }

        public DateTime fchComentario { get; set; }

        public Usuarios usuarios { get; set; }
        public Publicaciones publicaciones { get; set; }
    }
}
