using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAPI.Model_Additional;
using RESTAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI.Controllers.v1
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class NoteController : Controller
    {
        private readonly DataContext _context;
        public NoteController(DataContext context)
        {
            _context = context;

            if (!_context.Notes.Any())
            {
                var newNote = new Note
                {
                    Title = "Hello world",
                    Content = "Welcome to the world",
               
                };
                _context.Add(newNote);
                _context.SaveChanges();
            }

        }

        //GET api/v1/Note
        [HttpGet]
        public async Task <ActionResult> GetAll()
        {

            List<Note> listNote = _context.Notes.ToList();
            List<Note_View> listView = new List<Note_View>();
            foreach (Note note in listNote)
            {
                var newNoteView = new Note_View
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    Created = (DateTime)_context.Entry(note).Property("CreatedDate").CurrentValue,
                    Modified = (DateTime)_context.Entry(note).Property("ModifiedDate").CurrentValue
                };
                listView.Add(newNoteView);
            }

            return Ok(listView);
        }

        //GET api/v1/Note/n
        [HttpGet("{id}")]
        public async Task<ActionResult> GetItem(int id)
        {
            var noteItem = _context.Notes.Find(id);
            if (noteItem == null)
            {
                return NotFound();
            }
            return Ok(noteItem);
        }

        //POST api/v1/Note
        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody]Note_Parameter newNote)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Note newOne = new Note
            {
                Title = newNote.Title,
                Content = newNote.Content
            };
            _context.Notes.Add(newOne);
            _context.SaveChanges();

            //add note first version
            var newVersion = new NoteVersion
            {
                NoteId = newOne.Id,
                version = 1,
                Title = newOne.Title,
                Content = newNote.Content,
                Created = (DateTime)_context.Entry(newOne).Property("CreatedDate").CurrentValue,
                Modified = (DateTime)_context.Entry(newOne).Property("ModifiedDate").CurrentValue

            };

            _context.NotesVersions.Add(newVersion);
            _context.SaveChanges();
            return CreatedAtAction("GetItem", new Note { Id = newOne.Id }, newNote);
        }


        //PUT api/v1/Note/n
        [HttpPut("{id}")]
        public async Task<ActionResult> PutNoteItem(int id, [FromBody]Note_Parameter note)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var changed = new Note
            {
                Id = id,
                Title = note.Title,
                Content = note.Content
            };
            _context.Notes.Attach(changed);
            _context.Entry(changed).State = EntityState.Modified;
            _context.SaveChanges();

            //add new version
            var newVersion = new NoteVersion
            {
                NoteId = changed.Id,
                version = _context.NotesVersions.Where(n => n.NoteId == changed.Id).Max(n => n.version) + 1,
                Title = changed.Title,
                Content = changed.Content,
                Created = _context.NotesVersions.Where(n => n.NoteId == changed.Id).Select(n => n.Created).First(),
                Modified = (DateTime)_context.Entry(changed).Property("ModifiedDate").CurrentValue

            };
            _context.NotesVersions.Add(newVersion);
            _context.SaveChanges();
            return Ok();
        }


        //DELETE api/v1/Note/n
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Attach(note);
            _context.Notes.Remove(note);
            _context.SaveChanges();
            return Ok();
        }

        //GET api/v1/Note/history/n
        [HttpGet("history/{Id}")]
        public async Task<ActionResult> GetNoteHistory(int Id)
        {
            
            var foundNoteParent = _context.NotesVersions.Where(n => n.NoteId == Id).Any();
            if (!foundNoteParent)
            {
                return NotFound();
            }
            
   
            var noteversion = _context.NotesVersions.Where(n => n.NoteId == Id).ToList();
            List<NoteVersion> listVersion = new List<NoteVersion>(noteversion);
            return Ok(listVersion);
        }

        //GET api/v1/Note/history
        [HttpGet("history")]
        public async Task<ActionResult> GetHistory()
        {
            var noteversion = _context.NotesVersions;
            List<NoteVersion> listVersion = new List<NoteVersion>(noteversion);
            return Ok(listVersion);
        }
    }
}
