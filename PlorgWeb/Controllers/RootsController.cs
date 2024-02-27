using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Plorg.Manager;
using Plorg.Model;
using PlorgRepo.Repo.ElementRepos;
using PlorgRepo.Repo.DBAccess;
using PlorgWeb.Security;
using PlorgWeb.WebDTO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;

namespace PlorgWeb.Controllers {
  [Route("api/v1/roots")]
  [ApiController]
  public class RootsController : ControllerBase {

    public RootElementManager RootElementManager { get; set; }
    public BaseElementManager BaseElementManager { get; set; }
    public JournalElementManager JournalElementManager { get; set; }
    public ChecklistElementManager ChecklistElementManager { get; set; }
    public RelationElementManager RelationElementManager { get; set; }
    public TimeLimitElementManager TimeLimitElementManager { get; set; }

    public RootsController() {

      var postgresAccess = new PostgresAccess();
      postgresAccess.connString = System.Configuration.ConfigurationManager.ConnectionStrings["postgres"].ConnectionString;
      var readonlyModeSettingString = System.Configuration.ConfigurationManager.AppSettings["readonlyMode"];

      bool readonlyMode;
      if (!bool.TryParse(readonlyModeSettingString, out readonlyMode))
        throw new Exception("Incorrect readonly mode setting (App.config)");

      postgresAccess.InitializeDatabase(readonlyMode);

      RootElementManager = new RootElementManager(new RootElementRepo(postgresAccess));
      BaseElementManager = new BaseElementManager(new BaseElementRepo(postgresAccess));
      JournalElementManager = new JournalElementManager(new JournalElementRepo(postgresAccess));
      ChecklistElementManager = new ChecklistElementManager(new ChecklistElementRepo(postgresAccess));
      RelationElementManager = new RelationElementManager(new RelationElementRepo(postgresAccess));
      TimeLimitElementManager = new TimeLimitElementManager(new TimeLimitElementRepo(postgresAccess));
    }

    /// <summary>
    /// Получить root
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    /// <responce code="200">Успешно</responce>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RootWebElement>), 200)]
    public IActionResult GetRoots([FromQuery] int limit = 5, [FromQuery] int offset = 0) {
      List<RootElement> elements = RootElementManager.Find(Guid.Empty).ToList();
      var elementDTOs = elements.Select(e => new RootWebElement(e)).Skip(offset).Take(limit);

      return Ok(elementDTOs);
    }

    /// <summary>
    /// Получить конкретный root
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [HttpGet("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(RootWebElement), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult Get(string id) {
      if (User.Identities.Where(x => x.Name == id).FirstOrDefault() == null)
        return Unauthorized();

      Guid rid;
      if (!Guid.TryParse(id, out rid))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { rid }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      var element = new RootWebElement(root);

      return Ok(element);
    }

    /// <summary>
    /// Добавить новый root
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <responce code="200">root успешно создан</responce>
    [HttpPost]
    [ProducesResponseType(typeof(RootWebElement), 200)]
    public IActionResult PostRoot([FromBody] RootWebElementDTO value) {
      value.name = value.name == null ? "" : value.name;
      value.password = value.password == null ? "" : value.password;

      var root = RootElementManager.Create(value.name, value.password);
      var rootWeb = new RootWebElement();
      rootWeb.ToWebElement(root);

      return Ok(rootWeb);
    }

    // PATCH api/<RootsController>/5
    /// <summary>
    /// Изменить root
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [HttpPatch("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(RootWebElement), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult PatchRoot(string id, [FromBody] RootWebElementDTO value) {
      if (User.Identities.Where(x => x.Name == id).FirstOrDefault() == null)
        return Unauthorized();

      Guid rid;
      if (!Guid.TryParse(id, out rid))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { rid }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      root.Name = value.name == null ? root.Name : value.name;
      root.Password = value.password == null ? root.Password : value.password;

      RootElementManager.Save(Guid.Empty, root);
      return Ok(new RootWebElement(root));
    }

    // DELETE api/<RootsController>/5
    /// <summary>
    /// Удалить root
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [ProducesResponseType(typeof(OkResult), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
    public IActionResult DeleteRoot(string id) {
      if (User.Identities.Where(x => x.Name == id).FirstOrDefault() == null)
        return Unauthorized();

      Guid rid;
      if (!Guid.TryParse(id, out rid))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { rid }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      RootElementManager.Delete(Guid.Empty, root.ID);
      return Ok();
    }

    /// <summary>
    /// Аутентификация
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Авторизация неуспешна</responce>
    /// <responce code="404">root не найден</responce>
    [HttpGet("{id}/auth")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult AuthenticateRoot(string id, [FromQuery] string password) {
      Guid rid;
      if (!Guid.TryParse(id, out rid))
        return BadRequest();
      /*
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { rid }).FirstOrDefault();

      if (root == null) {
          return NotFound();
      }*/

      var root = RootElementManager.Authenticate(rid, password);
      if (root == null) {
        return Unauthorized();
      }

      var claims = new List<Claim> { new Claim(ClaimTypes.Name, id) };

      var jwt = new JwtSecurityToken(
              issuer: JWTAuthOptions.ISSUER,
              audience: JWTAuthOptions.AUDIENCE,
              claims: claims,
              expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
              signingCredentials: new SigningCredentials(JWTAuthOptions.KEY, SecurityAlgorithms.HmacSha256));

      return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    /// <summary>
    /// Получение элементов задач
    /// </summary>
    /// <param name="rid"></param>
    /// <returns></returns>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    /// <responce code="500">Ошибка сервера</responce>
    [HttpGet("{rid}/tasks"), Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(TaskElements), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult GetTasks(string rid) {
      if (User.Identities.Where(x => x.Name == rid).FirstOrDefault() == null)
        return Unauthorized();

      Guid RID;
      if (!Guid.TryParse(rid, out RID))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { RID }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      var bases = BaseElementManager.Find(root.ID);
      TaskElements tasks = new TaskElements();

      if (bases.Count() == 0) {
        return Ok(tasks);
      }

      tasks.baseElements = bases.Select(x => new BaseWebElement(x)).ToList();
      tasks.journals = JournalElementManager.Find(root.ID).Select(x => new JournalWebElement(x)).ToList();
      tasks.checklists = ChecklistElementManager.Find(root.ID).Select(x => new ChecklistWebElement(x)).ToList();
      tasks.relations = RelationElementManager.Find(root.ID).Select(x => new RelationWebElement(x)).ToList();
      tasks.timeLimits = TimeLimitElementManager.Find(root.ID).Select(x => new TimeLimitWebElement(x)).ToList();

      return Ok(tasks);
    }

    /// <summary>
    /// Добавление новой базы задачи
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="baseDTO"></param>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [HttpPost("{rid}/tasks"), Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(BaseWebElement), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult PostTask(string rid, [FromBody] BaseWebElementDTO baseDTO) {
      if (User.Identities.Where(x => x.Name == rid).FirstOrDefault() == null)
        return Unauthorized();

      Guid RID;
      if (!Guid.TryParse(rid, out RID))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { RID }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      var webElement = baseDTO.FromDTO();

      var newBase = new BaseElement(Guid.NewGuid(), root.ID, webElement.Name, webElement.Description);

      BaseElementManager.Save(root.ID, newBase);

      webElement.ToWebElement(newBase);

      return Ok(webElement);
    }

    /// <summary>
    /// Удаление задачи
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="bid"></param>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [HttpDelete("{rid}/tasks/{bid}"), Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(BadRequestResult), 404)]
    public IActionResult DeleteTask(string rid, string bid) {
      if (User.Identities.Where(x => x.Name == rid).FirstOrDefault() == null)
        return Unauthorized();

      Guid RID;
      if (!Guid.TryParse(rid, out RID))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { RID }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      Guid BID;
      if (!Guid.TryParse(bid, out BID))
        return BadRequest();

      BaseElementManager.Delete(root.ID, BID);

      return Ok();
    }

    /// <summary>
    /// Изменение задачи
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="tasks"></param>
    /// <response code="200">Успешно</response>
    /// <responce code="400">Некорректный формат id</responce>
    /// <responce code="401">Auth error</responce>
    /// <responce code="404">root не найден</responce>
    [HttpPatch("{rid}/tasks/"), Authorize(AuthenticationSchemes = "Bearer")]
    public IActionResult PatchTasks(string rid, [FromBody] TaskElements tasks) {
      if (User.Identities.Where(x => x.Name == rid).FirstOrDefault() == null)
        return Unauthorized();

      Guid RID;
      if (!Guid.TryParse(rid, out RID))
        return BadRequest();
      var root = RootElementManager.Find(Guid.Empty, new Guid[] { RID }).FirstOrDefault();

      if (root == null) {
        return NotFound();
      }

      // update bases
      var newBases = tasks.baseElements.Select(x => x.ToElement()).ToList();

      BaseElementManager.Save(root.RID, newBases);
      var basesGuids = newBases.Select(x => x.BID).ToArray();

      // update journals
      var journals = JournalElementManager.Find(root.ID).Where(x => basesGuids.Contains(x.BID));
      var newJournals = tasks.journals.Select(x => x.ToElement()).ToList();

      var journalsDelete = journals.Where(x => !newJournals.Exists(y => y.ID == x.ID));

      JournalElementManager.Save(root.RID, newJournals);
      JournalElementManager.Delete(root.ID, journalsDelete.Select(x => x.ID));

      // update checklists
      var checklists = ChecklistElementManager.Find(root.ID).Where(x => basesGuids.Contains(x.BID));
      var newChecklists = tasks.checklists.Select(x => x.ToElement()).ToList();

      var checklistsDelete = checklists.Where(x => !newChecklists.Exists(y => y.ID == x.ID));

      ChecklistElementManager.Save(root.RID, newChecklists);
      ChecklistElementManager.Delete(root.ID, checklistsDelete.Select(x => x.ID));

      // update relations
      var relations = RelationElementManager.Find(root.ID).Where(x => basesGuids.Contains(x.BID));
      var newRelations = tasks.relations.Select(x => x.ToElement()).ToList();

      var relationsDelete = relations.Where(x => !newRelations.Exists(y => y.ID == x.ID));

      RelationElementManager.Save(root.RID, newRelations);
      RelationElementManager.Delete(root.ID, journalsDelete.Select(x => x.ID));

      // update time limits
      var timeLimits = TimeLimitElementManager.Find(root.ID).Where(x => basesGuids.Contains(x.BID));
      var newTimeLimits = tasks.timeLimits.Select(x => x.ToElement()).ToList();

      var timeLimitsDelete = timeLimits.Where(x => !newTimeLimits.Exists(y => y.ID == x.ID));

      TimeLimitElementManager.Save(root.RID, newTimeLimits);
      TimeLimitElementManager.Delete(root.ID, timeLimitsDelete.Select(x => x.ID));

      return Ok();
    }
  }
}
