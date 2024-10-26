//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.LibraryService.Models;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 649 // Disable "CS0649 Field is never assigned to, and will always have its default value null"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"
#pragma warning disable 8604 // Disable "CS8604 Possible null reference argument for parameter"
#pragma warning disable 8625 // Disable "CS8625 Cannot convert null literal to non-nullable reference type"
#pragma warning disable 8765 // Disable "CS8765 Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes)."

namespace LibraryAPI.LibraryService
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface ILibraryService
    {

        /// <summary>
        /// Get all books
        /// </summary>

        /// <remarks>
        /// Retrieve a list of all books in the library.
        /// </remarks>

        /// <returns>OK</returns>

        System.Threading.Tasks.Task<System.Collections.Generic.ICollection<BookModel>> GetBooksAsync();

        /// <summary>
        /// Add a new book
        /// </summary>

        /// <remarks>
        /// Add a new book to the library.
        /// </remarks>

        /// <returns>Created</returns>

        System.Threading.Tasks.Task<BookModel> PostBookAsync(Book body);

        /// <summary>
        /// Get a specific book
        /// </summary>

        /// <remarks>
        /// Retrieve a specific book by its ID.
        /// </remarks>

        /// <returns>OK</returns>

        Task<BookModel?> GetBookAsync(int id);

        /// <summary>
        /// Update a book
        /// </summary>

        /// <remarks>
        /// Update an existing book by its ID.
        /// </remarks>



        /// <returns>OK</returns>

        System.Threading.Tasks.Task<BookModel> PutBookAsync(int id, Book body);

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class LibraryController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private ILibraryService _libraryService;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryService libraryService, IMapper mapper)
        {
            _libraryService = libraryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <remarks>
        /// Retrieve a list of all books in the library.
        /// </remarks>
        /// <returns>OK</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/books")]
        public async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<BookWithId>> GetBooks()
        {

           var books = await _libraryService.GetBooksAsync();
           return _mapper.Map<ICollection<BookWithId>>(books as ICollection<BookModel>);
        }

        /// <summary>
        /// Add a new book
        /// </summary>
        /// <remarks>
        /// Add a new book to the library.
        /// </remarks>
        /// <returns>Created</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("api/book")]
        public async System.Threading.Tasks.Task<BookWithId> PostBook([Microsoft.AspNetCore.Mvc.FromBody] Book body)
        {

            return _mapper.Map<BookWithId>(await _libraryService.PostBookAsync(body));
        }

        /// <summary>
        /// Get a specific book
        /// </summary>
        /// <remarks>
        /// Retrieve a specific book by its ID.
        /// </remarks>
        /// <returns>OK</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("api/book/{id}")]
        public async System.Threading.Tasks.Task<BookWithId> GetBook(int id)
        {

            return _mapper.Map<BookWithId>(await _libraryService.GetBookAsync(id));
        }

        /// <summary>
        /// Update a book
        /// </summary>
        /// <remarks>
        /// Update an existing book by its ID.
        /// </remarks>
        /// <returns>OK</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut, Microsoft.AspNetCore.Mvc.Route("api/book/{id}")]
        public async System.Threading.Tasks.Task<BookWithId> PutBook(int id, [Microsoft.AspNetCore.Mvc.FromBody] Book body)
        {

            return _mapper.Map<BookWithId>(await _libraryService.PutBookAsync(id, body));
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Book
    {
        [Newtonsoft.Json.JsonProperty("title", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("author", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("isbn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Isbn { get; set; }

        [Newtonsoft.Json.JsonProperty("publishedDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset PublishedDate { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class BookWithId
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("title", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("author", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("isbn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Isbn { get; set; }

        [Newtonsoft.Json.JsonProperty("publishedDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset PublishedDate { get; set; }

    }   

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal class DateFormatConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        public DateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }


}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603
#pragma warning restore 8604
#pragma warning restore 8625