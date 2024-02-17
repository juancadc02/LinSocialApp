using DB;
using DB.Modelo;
using LinkSocial1.DTO;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace LinkSocial1.Servicios
{
    public class ServicioConsultasImpl:ServicioConsultas
    {
        private readonly GestorLinkSocialDbContext dbContext;

        public ServicioConsultasImpl(GestorLinkSocialDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServicioConsultasImpl()
        {
            dbContext = new GestorLinkSocialDbContext();
        }
        public bool IniciarSesion(string correoElectronico, string contraseña, out int idUsuario, out string rolUsuario)
        {
            ServicioEncriptar encriptarContraseña = new ServicioEncriptarImpl();

            var usuario = dbContext.Usuarios.FirstOrDefault(u => u.correoElectronico == correoElectronico && u.contraseña == encriptarContraseña.Encriptar(contraseña));

            if (usuario == null)
            {
                idUsuario = 0; // Usuario no encontrado, asignamos un valor por defecto
                rolUsuario = null; // No hay rol que devolver
                return false;
            }
            else
            {
                idUsuario = usuario.idUsuario; // Asignamos el ID del usuario encontrado
                rolUsuario = usuario.rolAcceso;
                return true;
            }
        }


        public void registrarUsuario(Usuarios nuevoUsuario)
        {
            ServicioEncriptar encriptar = new ServicioEncriptarImpl();
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoUsuario = new Usuarios
                {
                    nombreCompleto=nuevoUsuario.nombreCompleto,
                    correoElectronico=nuevoUsuario.correoElectronico,
                    dniUsuario=nuevoUsuario.dniUsuario,
                    movilUsuario=nuevoUsuario.movilUsuario,
                    contraseña=encriptar.Encriptar(nuevoUsuario.contraseña),
                    fchNacimiento=nuevoUsuario.fchNacimiento,
                    fchRegistro=nuevoUsuario.fchRegistro,
                    rolAcceso=nuevoUsuario.rolAcceso,
                    rutaImagen=nuevoUsuario.rutaImagen
                };

                contexto.Usuarios.Add(nuevoUsuario);
                contexto.SaveChanges();
            }
        }
        public bool existeCorreoElectronico(string correoElectronico)
        {
            return dbContext.Usuarios.Any(u => u.correoElectronico == correoElectronico);
        }
        public bool existeDNI(string dni)
        {
            return dbContext.Usuarios.Any(u => u.dniUsuario == dni);
        }
        public List<Usuarios> mostrarUsuarios()
        {

            try
            {
                using (var contexto = new GestorLinkSocialDbContext())
                {
                    List<Usuarios> listaUsuarios = contexto.Usuarios.ToList();

                    if (listaUsuarios.Count == 0)
                    {
                        Console.WriteLine("No existe ningun usuario");
                    }
                    else
                    {
                        foreach (var usu in listaUsuarios)
                        {
                            Console.WriteLine("\n\n\t{0}   {1}  {2} {3} {4} {5}", usu.nombreCompleto, usu.correoElectronico, usu.dniUsuario, usu.movilUsuario, usu.fchNacimiento, usu.rolAcceso);
                        }
                    }
                    return listaUsuarios;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Se ha producido un error:{0}",ex);
                return null;
            }
            
        }
        public void EnviarEmail(string emailDestino, string nombreUser, string token)
        {
            string urlDominio = "https://localhost:7294";

            string EmailOrigen = "juanccaaa15@gmail.com";
            //Se crea la URL de recuperación con el token que se enviará al mail del user.
            string urlDeRecuperacion = String.Format("{0}/ControladorRecuperarContraseña/Recuperar/?token={1}", urlDominio, token);

            //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
            string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
            string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/RecuperacionContraseñaCorreo.html");
            string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
            //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
            htmlContent = String.Format(htmlContent, nombreUser, urlDeRecuperacion);

            MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "RESTABLECER CONTRASEÑA", htmlContent);

            mensajeDelCorreo.IsBodyHtml = true;

            SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
            smtpCliente.EnableSsl = true;
            smtpCliente.UseDefaultCredentials = false;
            smtpCliente.Port = 587;
            smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "mepy ftvq rbui vmqq" +
                "");

            smtpCliente.Send(mensajeDelCorreo);

            smtpCliente.Dispose();
        }

        public void subirPublicacion(Publicaciones nuevaPublicacion)
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevaPublicacion = new Publicaciones
                {
                   idUsuario=nuevaPublicacion.idUsuario,
                    fchPublicacion=nuevaPublicacion.fchPublicacion,
                    contenidoPublicacion=nuevaPublicacion.contenidoPublicacion
                };

                contexto.Add(nuevaPublicacion);
                contexto.SaveChanges();
                Console.WriteLine("Nueva publicacion guardada.");
            }
        }

        public List<Publicaciones> mostrarPublicaciones()
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                var listaPublicaciones =contexto.Publicaciones.ToList();

                foreach(var publi in listaPublicaciones)
                {
                    Console.WriteLine("{0}",publi.contenidoPublicacion);
                }
                return listaPublicaciones;
            }
        }

        public void añadirComentario(Comentarios nuevoComentario)
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoComentario = new Comentarios
                {
                    idUsuario=nuevoComentario.idUsuario,
                    idPublicacion=nuevoComentario.idPublicacion,
                    contenidoComentario=nuevoComentario.contenidoComentario,
                    fchComentario=nuevoComentario.fchComentario
                };

                contexto.Comentarios.Add(nuevoComentario);
                contexto.SaveChanges();
            }
        }

        public List<ComentarioConUsuarioViewModel> mostrarComentariosConUsuario()
        {
            using (var contexto = new GestorLinkSocialDbContext())
            {
                var listaComentarios = contexto.Comentarios.ToList();
                var comentariosConUsuario = new List<ComentarioConUsuarioViewModel>();

                foreach (var comentario in listaComentarios)
                {
                    var usuarioComentario = contexto.Usuarios.FirstOrDefault(u => u.idUsuario == comentario.idUsuario);

                    // Si el usuario no es null, agregar el comentario con el usuario a la lista
                    if (usuarioComentario != null)
                    {
                        comentariosConUsuario.Add(new ComentarioConUsuarioViewModel
                        {
                            Comentario = comentario,
                            Usuario = usuarioComentario
                        });
                    }
                }
                return comentariosConUsuario;
            }
        }

        public Usuarios buscarUsuario(string correoElectronico)
        {

            using(var contexto = new GestorLinkSocialDbContext())
            {
                Usuarios usuarioEncontrado = contexto.Usuarios.FirstOrDefault(u => u.correoElectronico == correoElectronico);

                if (usuarioEncontrado!=null)
                {
                    return usuarioEncontrado;
                }
                else
                {
                    Console.WriteLine("Usuario no encontrado");
                    return null;
                }
                
            }
        }
        public Usuarios buscarUsuarioPorId(int idUsuario)
        {
            using (var contexto = new GestorLinkSocialDbContext())
            {
                Usuarios usuarioEncontrado = contexto.Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

                if (usuarioEncontrado != null)
                {
                    return usuarioEncontrado;
                }
                else
                {
                    Console.WriteLine("Usuario no encontrado");
                    return null;
                }

            }
        }
        public List<Publicaciones> buscarPublicacionesPorIdUsuario(int idUsuario)
        {
            using (var contexto = new GestorLinkSocialDbContext())
            {
                var publicacionesDelUsuario = dbContext.Publicaciones
                                               .Where(p => p.idUsuario == idUsuario)
                                               .ToList();

                foreach (var publicaciones in publicacionesDelUsuario)
                {
                    Console.WriteLine("{0}", publicaciones.contenidoPublicacion);
                }
                return publicacionesDelUsuario;
            }
        }

        public int ObtenerNumeroSeguidores(int idUsuario)
        {
            int numeroSeguidores = dbContext.Seguidores
                .Where(s => s.idSeguidorSeguido == idUsuario && s.siguiendo)
                .Count();

            return numeroSeguidores;
        }

        public int ObtenerNumeroSeguidos(int idUsuario)
        {
            int numeroSeguidos = dbContext.Seguidores
                .Where(s => s.idSeguidorSolicitud == idUsuario && s.siguiendo)
                .Count();

            return numeroSeguidos;
        }

        public void iniciarSeguimiento(Seguidores nuevoSeguidor)
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoSeguidor = new Seguidores
                {
                    idSeguidorSolicitud=nuevoSeguidor.idSeguidorSolicitud,
                    idSeguidorSeguido=nuevoSeguidor.idSeguidorSeguido,
                    fchSeguimiento=nuevoSeguidor.fchSeguimiento,
                    siguiendo=nuevoSeguidor.siguiendo
                };

                contexto.Seguidores.Add(nuevoSeguidor);
                contexto.SaveChanges();
            }
        }
        public void dejarDeSeguir(int idSeguidorSolicitud, int idSeguidorSeguido)
        {
            using (var dbContext = new GestorLinkSocialDbContext())
            {
                // Buscar la relación de seguimiento
                var seguimiento = dbContext.Seguidores
                    .FirstOrDefault(s => s.idSeguidorSolicitud == idSeguidorSolicitud && s.idSeguidorSeguido == idSeguidorSeguido);

                if (seguimiento != null)
                {
                    // Si la relación de seguimiento existe, eliminarla
                    dbContext.Seguidores.Remove(seguimiento);
                    dbContext.SaveChanges();
                }
                // Puedes manejar una lógica adicional si la relación de seguimiento no existe
            }
        }
        public bool estaSiguiendo(int idSeguidorSolicitud, int idSeguidorSeguido)
        {
            using (var dbContext = new GestorLinkSocialDbContext())
            {
                // Verificar si ya existe una relación de seguimiento
                var existeSeguimiento = dbContext.Seguidores
                    .Any(s => s.idSeguidorSolicitud == idSeguidorSolicitud
                           && s.idSeguidorSeguido == idSeguidorSeguido
                           && s.siguiendo);

                return existeSeguimiento;
            }
        }


        public void añadirLike(LikeUsuariosPublicaciones nuevoLike)
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoLike = new LikeUsuariosPublicaciones
                {
                    idUsuario=nuevoLike.idUsuario,
                    idPublicacion=nuevoLike.idPublicacion,
                    fchLike=nuevoLike.fchLike
                };

                contexto.LikeUsuariosPublicaciones.Add(nuevoLike);
                contexto.SaveChanges();
            }
        }

        public bool usuarioDioLike(int idUsuario, int idPublicacion)
        {
            using (var dbContext = new GestorLinkSocialDbContext())
            {
                // Supongamos que tienes una entidad Likes con campos idUsuario y idPublicacion
                var likeExistente = dbContext.LikeUsuariosPublicaciones
                    .FirstOrDefault(l => l.idUsuario == idUsuario && l.idPublicacion == idPublicacion);

                // Devuelve true si el usuario ya dio "me gusta", false en caso contrario
                return likeExistente != null;
            }
        }

        public void eliminarLike(int idUsuario, int idPublicacion)
        {
            // Lógica para eliminar un "me gusta" de la publicación
            using (var dbContext = new GestorLinkSocialDbContext())
            {
                var likeExistente = dbContext.LikeUsuariosPublicaciones
                    .FirstOrDefault(l => l.idUsuario == idUsuario && l.idPublicacion == idPublicacion);

                if (likeExistente != null)
                {
                    dbContext.LikeUsuariosPublicaciones.Remove(likeExistente);
                    dbContext.SaveChanges();
                }
            }
        }


        public List<Mensajes> ObtenerHistorialMensajes(string idUsuarioActual, int idUsuarioDestino)
        {
            // Obtén el historial de mensajes entre el usuario actual y el usuario destino
            List<Mensajes> historialMensajes = dbContext.Mensajes
                .Include(m => m.usuariosEnvia) // Incluye el objeto UsuarioQueEnvia en la consulta
                .Where(m =>
                    (m.idUsuarioQueEnvia == Convert.ToInt32(idUsuarioActual) && m.idUsuarioQueRecibe == idUsuarioDestino) ||
                    (m.idUsuarioQueEnvia == idUsuarioDestino && m.idUsuarioQueRecibe == Convert.ToInt32(idUsuarioActual))
                )
                .OrderBy(m => m.fchEnvioMensaje)
                .ToList();

            return historialMensajes;
        }

        public void enviarMensaje(Mensajes nuevoMensaje)
        {
            using(var contexto = new GestorLinkSocialDbContext())
            {
                nuevoMensaje = new Mensajes
                {
                    idUsuarioQueEnvia=nuevoMensaje.idUsuarioQueEnvia,
                    idUsuarioQueRecibe=nuevoMensaje.idUsuarioQueRecibe,
                    contenidoMensaje=nuevoMensaje.contenidoMensaje,
                    fchEnvioMensaje=nuevoMensaje.fchEnvioMensaje
                };

                contexto.Mensajes.Add(nuevoMensaje);
                contexto.SaveChanges();
            }
        }


    }
}
