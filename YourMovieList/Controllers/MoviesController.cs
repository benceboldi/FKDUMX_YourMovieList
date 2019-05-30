using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MoviesDataAccess;

namespace YourMovieList.Controllers
{
    public class MoviesController : ApiController
    {
        public IEnumerable<Movy> Get()
        {
            using (YourMovieListDBEntities entities = new YourMovieListDBEntities())
            {
                return entities.Movies.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (YourMovieListDBEntities entities = new YourMovieListDBEntities())
            {
                var entity = entities.Movies.FirstOrDefault(e => e.movieID == id);

                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Movie with Id = " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Movy movie)
        {
            try
            {
                using(YourMovieListDBEntities entities = new YourMovieListDBEntities())
                {
                    entities.Movies.Add(movie);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, movie);
                    message.Headers.Location = new Uri(Request.RequestUri + movie.movieID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (YourMovieListDBEntities entities = new YourMovieListDBEntities())
                {
                    var entity = entities.Movies.FirstOrDefault(e => e.movieID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " could not be deleted because it does not exist");
                    }
                    else
                    {
                        entities.Movies.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Movy movie)
        {
            try
            {
                using (YourMovieListDBEntities entities = new YourMovieListDBEntities())
                {
                    var entity = entities.Movies.FirstOrDefault(e => e.movieID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Movie with Id = " + id.ToString() + "not found");
                    }
                    else
                    {
                        entity.title = movie.title;
                        entity.language = movie.language;
                        entity.length = movie.length;
                        entity.imdb = movie.imdb;
                        entity.director = movie.director;
                        entity.genre = movie.genre;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
