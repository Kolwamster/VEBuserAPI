using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VEBuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IDisposable
    {
        private UserContext _db;

        public UserController()
        {
            _db = new UserContext();
        }

        // GET: api/<UserController>
        [HttpGet("{pageSize}-{pageIndex}")]
        public IEnumerable<object> Get(int pageSize, int pageIndex)
        {
            var users = _db.Users.Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                        .SelectMany(u => u.Roles, (u, r) =>
                            new {
                                Id = u.Id,
                                Name = u.Name,
                                Age = u.Age,
                                Email = u.Email,
                                Roles = u.Roles
                            }
                        )
                        .ToList();
            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public object Get(Guid id)
        {
            var users = _db.Users
                        .Where(u => u.Id == id)
                        .SelectMany(u => u.Roles, (u, i) =>
                            new {
                                Name = u.Name,
                                Age = u.Age,
                                Email = u.Email,
                                Roles = u.Roles
                            }
                        )
                        .ToList();
            return users.First();
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] User value)
        {
            User user = new User();
            user.Id = Guid.NewGuid();
            user.Name = value.Name;
            user.Age = value.Age;
            user.Email = value.Email;

            _db.Users.Add(user);
            _db.SaveChanges();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] User value)
        {
            User user = new User();
            user.Id = id;
            user.Name = value.Name;
            user.Age = value.Age;
            user.Email = value.Email;
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        // PUT api/<UserController>/AddRole/5
        [HttpPatch("AddRole/{id}")]
        public void Patch(Guid id, [FromBody] Role value) 
        {
            User? user = _db.Users.Find(id);
            Role? role = _db.Roles.Find(value.Id);
            if (user is not null && role is not null)
            {
                user.Roles.Add(role);
                _db.Users.Update(user);
                _db.SaveChanges();
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            User user = new User();
            user.Id = id;
            _db.Remove(user);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
