using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VEBuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IDisposable
    {
        private DataContext _db;

        private void ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentNullException(nameof(user.Name));
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }

            if (user.Age <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(user.Age));
            }
        }

        private void ValidateEmail(User user)
        {
            if (_db.Users.Count(u => u.Email == user.Email) > 0)
            {
                throw new ArgumentException($"{user.Email} is already in use.");
            }
        }

        public UserController(IConfiguration config)
        {
            _db = new DataContext(config);
        }

        // GET: api/<UserController>
        [HttpGet]
        public IActionResult GetUsers(int pageSize, int pageIndex, string? userName = "", int? age = 0, string? email = "", string? roleName = "")
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                return BadRequest();
            }

            if (age < 0)
            {
                return BadRequest();
            }

            try
            {
                var users = _db.Users
                        .Include(user => user.Roles)
                        .Where(u => string.IsNullOrEmpty(userName) || u.Name.Contains(userName))
                        .Where(u => string.IsNullOrEmpty(email) || u.Email.Contains(email))
                        .Where(u => age == 0 || u.Age == age)
                        .Where(u => string.IsNullOrEmpty(roleName) || u.Roles.Any(r => r.Name.Contains(roleName)));

                if (!users.Any())
                {
                    return NotFound();
                }

                var page = users.Skip(pageSize * (pageIndex - 1))
                            .Take(pageSize)
                            .ToList();
                return Ok(page);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                var users = _db.Users
                        .Include(user => user.Roles)
                        .Where(u => u.Id == id);
                return users.Any() ? Ok(users.First()) : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult PostUser([FromBody] User value)
        {
            try
            {
                ValidateUser(value);
                ValidateEmail(value);
                value.Id = Guid.NewGuid();
                _db.Users.Add(value);
                _db.SaveChanges();
                return Ok(value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult PutUser(Guid id, [FromBody] User value)
        {
            try
            {
                ValidateUser(value);
                value.Id = id;
                _db.Users.Update(value);
                _db.SaveChanges();
                return Ok(value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<UserController>/AddRole/5
        [HttpPatch("AddRole/{id}")]
        public IActionResult AddUserRole(Guid id, [FromBody] Role value) 
        {
            User? user = _db.Users.Find(id);
            Role? role = _db.Roles.Find(value.Id);
            if (user is not null && role is not null)
            {
                try
                {
                    user.Roles.Add(role);
                    _db.Users.Update(user);
                    _db.SaveChanges();
                    return Ok(user);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            try
            {
                User user = new User();
                user.Id = id;
                _db.Remove(user);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
