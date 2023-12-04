using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Draco.Lsp.Attributes;
using Draco.Lsp.Serialization;

namespace Draco.Lsp.Model;
#nullable enable
#pragma warning disable CS9042
public sealed class ImplementationParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

/// <summary>
/// A parameter literal used in requests to pass a text document and a position inside that
/// document.
/// </summary>
public sealed class TextDocumentPositionParams : ITextDocumentPositionParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }
}

/// <summary>
/// A parameter literal used in requests to pass a text document and a position inside that
/// document.
/// </summary>
public interface ITextDocumentPositionParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public ITextDocumentIdentifier TextDocument { get; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Position Position { get; }
}

/// <summary>
/// A literal to identify a text document in the client.
/// </summary>
public sealed class TextDocumentIdentifier : ITextDocumentIdentifier
{
    /// <summary>
    /// The text document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }
}

/// <summary>
/// A literal to identify a text document in the client.
/// </summary>
public interface ITextDocumentIdentifier
{
    /// <summary>
    /// The text document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Draco.Lsp.Model.DocumentUri Uri { get; }
}

/// <summary>
/// Position in a text document expressed as zero-based line and character
/// offset. Prior to 3.17 the offsets were always based on a UTF-16 string
/// representation. So a string of the form `a𐐀b` the character offset of the
/// character `a` is 0, the character offset of `𐐀` is 1 and the character
/// offset of b is 3 since `𐐀` is represented using two code units in UTF-16.
/// Since 3.17 clients and servers can agree on a different string encoding
/// representation (e.g. UTF-8). The client announces it's supported encoding
/// via the client capability [`general.positionEncodings`](#clientCapabilities).
/// The value is an array of position encodings the client supports, with
/// decreasing preference (e.g. the encoding at index `0` is the most preferred
/// one). To stay backwards compatible the only mandatory encoding is UTF-16
/// represented via the string `utf-16`. The server can pick one of the
/// encodings offered by the client and signals that encoding back to the
/// client via the initialize result's property
/// [`capabilities.positionEncoding`](#serverCapabilities). If the string value
/// `utf-16` is missing from the client's capability `general.positionEncodings`
/// servers can safely assume that the client supports UTF-16. If the server
/// omits the position encoding in its initialize result the encoding defaults
/// to the string value `utf-16`. Implementation considerations: since the
/// conversion from one encoding into another requires the content of the
/// file / line the conversion is best done where the file is read which is
/// usually on the server side.
/// 
/// Positions are line end character agnostic. So you can not specify a position
/// that denotes `\r|\n` or `\n|` where `|` represents the character offset.
/// 
/// @since 3.17.0 - support for negotiated position encoding.
/// </summary>
public sealed class Position
{
    /// <summary>
    /// Line position in a document (zero-based).
    /// 
    /// If a line number is greater than the number of lines in a document, it defaults back to the number of lines in the document.
    /// If a line number is negative, it defaults to 0.
    /// </summary>
    [JsonPropertyName("line")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 Line { get; set; }

    /// <summary>
    /// Character offset on a line in a document (zero-based).
    /// 
    /// The meaning of this offset is determined by the negotiated
    /// `PositionEncodingKind`.
    /// 
    /// If the character value is greater than the line length it defaults back to the
    /// line length.
    /// </summary>
    [JsonPropertyName("character")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 Character { get; set; }
}

public sealed class WorkDoneProgressParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

public interface IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; }
}

public sealed class PartialResultParams : IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

public interface IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; }
}

/// <summary>
/// Represents a location inside a resource, such as a line
/// inside a text file.
/// </summary>
public sealed class Location
{
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }
}

/// <summary>
/// A range in a text document expressed as (zero-based) start and end positions.
/// 
/// If you want to specify a range that contains a line including the line ending
/// character(s) then use an end position denoting the start of the next line.
/// For example:
/// ```ts
/// {
///     start: { line: 5, character: 23 }
///     end : { line 6, character : 0 }
/// }
/// ```
/// </summary>
public sealed class Range
{
    /// <summary>
    /// The range's start position.
    /// </summary>
    [JsonPropertyName("start")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Start { get; set; }

    /// <summary>
    /// The range's end position.
    /// </summary>
    [JsonPropertyName("end")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position End { get; set; }
}

public sealed class ImplementationRegistrationOptions : ITextDocumentRegistrationOptions, IImplementationOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// General text document registration options.
/// </summary>
public sealed class TextDocumentRegistrationOptions : ITextDocumentRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// General text document registration options.
/// </summary>
public interface ITextDocumentRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public IList<DocumentFilter>? DocumentSelector { get; }
}

/// <summary>
/// A notebook cell text document filter denotes a cell text
/// document by different properties.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookCellTextDocumentFilter
{
    public sealed class NotebookFilter
    {
        /// <summary>
        /// The type of the enclosing notebook.
        /// </summary>
        [JsonPropertyName("notebookType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.String? NotebookType { get; set; }

        /// <summary>
        /// A Uri {@link Uri.scheme scheme}, like `file` or `untitled`.
        /// </summary>
        [JsonPropertyName("scheme")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.String? Scheme { get; set; }

        /// <summary>
        /// A glob pattern.
        /// </summary>
        [JsonPropertyName("pattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.String? Pattern { get; set; }
    }

    /// <summary>
    /// A filter that matches against the notebook
    /// containing the notebook cell. If a string
    /// value is provided it matches against the
    /// notebook type. '*' matches every notebook.
    /// </summary>
    [JsonPropertyName("notebook")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.String, NotebookCellTextDocumentFilter.NotebookFilter> Notebook { get; set; }

    /// <summary>
    /// A language id like `python`.
    /// 
    /// Will be matched against the language id of the
    /// notebook cell document. '*' matches every language.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Language { get; set; }
}

public sealed class DocumentFilter
{
    /// <summary>
    /// A language id, like `typescript`.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Language { get; set; }

    /// <summary>
    /// A Uri {@link Uri.scheme scheme}, like `file` or `untitled`.
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Scheme { get; set; }

    /// <summary>
    /// A glob pattern, like `*.{ts,js}`.
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Pattern { get; set; }
}

public sealed class ImplementationOptions : IImplementationOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IImplementationOptions
{
}

public sealed class WorkDoneProgressOptions : IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; }
}

/// <summary>
/// Static registration options to be returned in the initialize
/// request.
/// </summary>
public sealed class StaticRegistrationOptions : IStaticRegistrationOptions
{
    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Static registration options to be returned in the initialize
/// request.
/// </summary>
public interface IStaticRegistrationOptions
{
    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; }
}

public sealed class TypeDefinitionParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

public sealed class TypeDefinitionRegistrationOptions : ITextDocumentRegistrationOptions, ITypeDefinitionOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class TypeDefinitionOptions : ITypeDefinitionOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface ITypeDefinitionOptions
{
}

/// <summary>
/// A workspace folder inside a client.
/// </summary>
public sealed class WorkspaceFolder
{
    /// <summary>
    /// The associated URI for this workspace folder.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Uri { get; set; }

    /// <summary>
    /// The name of the workspace folder. Used to refer to this
    /// workspace folder in the user interface.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }
}

/// <summary>
/// The parameters of a `workspace/didChangeWorkspaceFolders` notification.
/// </summary>
public sealed class DidChangeWorkspaceFoldersParams
{
    /// <summary>
    /// The actual workspace folder change event.
    /// </summary>
    [JsonPropertyName("event")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required WorkspaceFoldersChangeEvent Event { get; set; }
}

/// <summary>
/// The workspace folder change event.
/// </summary>
public sealed class WorkspaceFoldersChangeEvent
{
    /// <summary>
    /// The array of added workspace folders
    /// </summary>
    [JsonPropertyName("added")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<WorkspaceFolder> Added { get; set; }

    /// <summary>
    /// The array of the removed workspace folders
    /// </summary>
    [JsonPropertyName("removed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<WorkspaceFolder> Removed { get; set; }
}

/// <summary>
/// The parameters of a configuration request.
/// </summary>
public sealed class ConfigurationParams
{
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<ConfigurationItem> Items { get; set; }
}

public sealed class ConfigurationItem
{
    /// <summary>
    /// The scope to get the configuration section for.
    /// </summary>
    [JsonPropertyName("scopeUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ScopeUri { get; set; }

    /// <summary>
    /// The configuration section asked for.
    /// </summary>
    [JsonPropertyName("section")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Section { get; set; }
}

/// <summary>
/// Parameters for a {@link DocumentColorRequest}.
/// </summary>
public sealed class DocumentColorParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// Represents a color range from a document.
/// </summary>
public sealed class ColorInformation
{
    /// <summary>
    /// The range in the document where this color appears.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The actual color value for this color range.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Color Color { get; set; }
}

/// <summary>
/// Represents a color in RGBA space.
/// </summary>
public sealed class Color
{
    /// <summary>
    /// The red component of this color in the range [0-1].
    /// </summary>
    [JsonPropertyName("red")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Double Red { get; set; }

    /// <summary>
    /// The green component of this color in the range [0-1].
    /// </summary>
    [JsonPropertyName("green")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Double Green { get; set; }

    /// <summary>
    /// The blue component of this color in the range [0-1].
    /// </summary>
    [JsonPropertyName("blue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Double Blue { get; set; }

    /// <summary>
    /// The alpha component of this color in the range [0-1].
    /// </summary>
    [JsonPropertyName("alpha")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Double Alpha { get; set; }
}

public sealed class DocumentColorRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentColorOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class DocumentColorOptions : IDocumentColorOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IDocumentColorOptions
{
}

/// <summary>
/// Parameters for a {@link ColorPresentationRequest}.
/// </summary>
public sealed class ColorPresentationParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The color to request presentations for.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Color Color { get; set; }

    /// <summary>
    /// The range where the color would be inserted. Serves as a context.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }
}

public sealed class ColorPresentation
{
    /// <summary>
    /// The label of this color presentation. It will be shown on the color
    /// picker header. By default this is also the text that is inserted when selecting
    /// this color presentation.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Label { get; set; }

    /// <summary>
    /// An {@link TextEdit edit} which is applied to a document when selecting
    /// this presentation for the color.  When `falsy` the {@link ColorPresentation.label label}
    /// is used.
    /// </summary>
    [JsonPropertyName("textEdit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ITextEdit? TextEdit { get; set; }

    /// <summary>
    /// An optional array of additional {@link TextEdit text edits} that are applied when
    /// selecting this color presentation. Edits must not overlap with the main {@link ColorPresentation.textEdit edit} nor with themselves.
    /// </summary>
    [JsonPropertyName("additionalTextEdits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ITextEdit>? AdditionalTextEdits { get; set; }
}

/// <summary>
/// A text edit applicable to a text document.
/// </summary>
public sealed class TextEdit : ITextEdit
{
    /// <summary>
    /// The range of the text document to be manipulated. To insert
    /// text into a document create a range where start === end.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The string to be inserted. For delete operations use an
    /// empty string.
    /// </summary>
    [JsonPropertyName("newText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NewText { get; set; }
}

/// <summary>
/// A text edit applicable to a text document.
/// </summary>
public interface ITextEdit
{
    /// <summary>
    /// The range of the text document to be manipulated. To insert
    /// text into a document create a range where start === end.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Range Range { get; }

    /// <summary>
    /// The string to be inserted. For delete operations use an
    /// empty string.
    /// </summary>
    [JsonPropertyName("newText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String NewText { get; }
}

/// <summary>
/// Parameters for a {@link FoldingRangeRequest}.
/// </summary>
public sealed class FoldingRangeParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// Represents a folding range. To be valid, start and end line must be bigger than zero and smaller
/// than the number of lines in the document. Clients are free to ignore invalid ranges.
/// </summary>
public sealed class FoldingRange
{
    /// <summary>
    /// The zero-based start line of the range to fold. The folded area starts after the line's last character.
    /// To be valid, the end must be zero or larger and smaller than the number of lines in the document.
    /// </summary>
    [JsonPropertyName("startLine")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 StartLine { get; set; }

    /// <summary>
    /// The zero-based character offset from where the folded range starts. If not defined, defaults to the length of the start line.
    /// </summary>
    [JsonPropertyName("startCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? StartCharacter { get; set; }

    /// <summary>
    /// The zero-based end line of the range to fold. The folded area ends with the line's last character.
    /// To be valid, the end must be zero or larger and smaller than the number of lines in the document.
    /// </summary>
    [JsonPropertyName("endLine")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 EndLine { get; set; }

    /// <summary>
    /// The zero-based character offset before the folded range ends. If not defined, defaults to the length of the end line.
    /// </summary>
    [JsonPropertyName("endCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? EndCharacter { get; set; }

    /// <summary>
    /// Describes the kind of the folding range such as `comment' or 'region'. The kind
    /// is used to categorize folding ranges and used by commands like 'Fold all comments'.
    /// See {@link FoldingRangeKind} for an enumeration of standardized kinds.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FoldingRangeKind? Kind { get; set; }

    /// <summary>
    /// The text that the client should show when the specified range is
    /// collapsed. If not defined or not supported by the client, a default
    /// will be chosen by the client.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("collapsedText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? CollapsedText { get; set; }
}

/// <summary>
/// A set of predefined range kinds.
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum FoldingRangeKind
{
    /// <summary>
    /// Folding range for a comment
    /// </summary>
    [EnumMember(Value = "comment")]
    Comment,
    /// <summary>
    /// Folding range for an import or include
    /// </summary>
    [EnumMember(Value = "imports")]
    Imports,
    /// <summary>
    /// Folding range for a region (e.g. `#region`)
    /// </summary>
    [EnumMember(Value = "region")]
    Region,
}

public sealed class FoldingRangeRegistrationOptions : ITextDocumentRegistrationOptions, IFoldingRangeOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class FoldingRangeOptions : IFoldingRangeOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IFoldingRangeOptions
{
}

public sealed class DeclarationParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

public sealed class DeclarationRegistrationOptions : IDeclarationOptions, ITextDocumentRegistrationOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class DeclarationOptions : IDeclarationOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IDeclarationOptions
{
}

/// <summary>
/// A parameter literal used in selection range requests.
/// </summary>
public sealed class SelectionRangeParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The positions inside the text document.
    /// </summary>
    [JsonPropertyName("positions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Position> Positions { get; set; }
}

/// <summary>
/// A selection range represents a part of a selection hierarchy. A selection range
/// may have a parent selection range that contains it.
/// </summary>
public sealed class SelectionRange
{
    /// <summary>
    /// The {@link Range range} of this selection range.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The parent selection range containing this range. Therefore `parent.range` must contain `this.range`.
    /// </summary>
    [JsonPropertyName("parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SelectionRange? Parent { get; set; }
}

public sealed class SelectionRangeRegistrationOptions : ISelectionRangeOptions, ITextDocumentRegistrationOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class SelectionRangeOptions : ISelectionRangeOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface ISelectionRangeOptions
{
}

public sealed class WorkDoneProgressCreateParams
{
    /// <summary>
    /// The token to be used to report progress.
    /// </summary>
    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.Int32, System.String> Token { get; set; }
}

public sealed class WorkDoneProgressCancelParams
{
    /// <summary>
    /// The token to be used to report progress.
    /// </summary>
    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.Int32, System.String> Token { get; set; }
}

/// <summary>
/// The parameter of a `textDocument/prepareCallHierarchy` request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyPrepareParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

/// <summary>
/// Represents programming constructs like functions or constructors in the context
/// of call hierarchy.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyItem
{
    /// <summary>
    /// The name of this item.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// The kind of this item.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this item.
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// More detail for this item, e.g. the signature of a function.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Detail { get; set; }

    /// <summary>
    /// The resource identifier of this item.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The range enclosing this symbol not including leading/trailing whitespace but everything else, e.g. comments and code.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The range that should be selected and revealed when this symbol is being picked, e.g. the name of a function.
    /// Must be contained by the {@link CallHierarchyItem.range `range`}.
    /// </summary>
    [JsonPropertyName("selectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range SelectionRange { get; set; }

    /// <summary>
    /// A data entry field that is preserved between a call hierarchy prepare and
    /// incoming calls or outgoing calls requests.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// A symbol kind.
/// </summary>
public enum SymbolKind
{
    File = 1,
    Module = 2,
    Namespace = 3,
    Package = 4,
    Class = 5,
    Method = 6,
    Property = 7,
    Field = 8,
    Constructor = 9,
    Enum = 10,
    Interface = 11,
    Function = 12,
    Variable = 13,
    Constant = 14,
    String = 15,
    Number = 16,
    Boolean = 17,
    Array = 18,
    Object = 19,
    Key = 20,
    Null = 21,
    EnumMember = 22,
    Struct = 23,
    Event = 24,
    Operator = 25,
    TypeParameter = 26,
}

/// <summary>
/// Symbol tags are extra annotations that tweak the rendering of a symbol.
/// 
/// @since 3.16
/// </summary>
public enum SymbolTag
{
    /// <summary>
    /// Render a symbol as obsolete, usually using a strike-out.
    /// </summary>
    Deprecated = 1,
}

/// <summary>
/// Call hierarchy options used during static or dynamic registration.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyRegistrationOptions : ITextDocumentRegistrationOptions, ICallHierarchyOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Call hierarchy options used during static registration.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyOptions : ICallHierarchyOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Call hierarchy options used during static registration.
/// 
/// @since 3.16.0
/// </summary>
public interface ICallHierarchyOptions
{
}

/// <summary>
/// The parameter of a `callHierarchy/incomingCalls` request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyIncomingCallsParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    [JsonPropertyName("item")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CallHierarchyItem Item { get; set; }
}

/// <summary>
/// Represents an incoming call, e.g. a caller of a method or constructor.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyIncomingCall
{
    /// <summary>
    /// The item that makes the call.
    /// </summary>
    [JsonPropertyName("from")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CallHierarchyItem From { get; set; }

    /// <summary>
    /// The ranges at which the calls appear. This is relative to the caller
    /// denoted by {@link CallHierarchyIncomingCall.from `this.from`}.
    /// </summary>
    [JsonPropertyName("fromRanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Range> FromRanges { get; set; }
}

/// <summary>
/// The parameter of a `callHierarchy/outgoingCalls` request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyOutgoingCallsParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    [JsonPropertyName("item")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CallHierarchyItem Item { get; set; }
}

/// <summary>
/// Represents an outgoing call, e.g. calling a getter from a method or a method from a constructor etc.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyOutgoingCall
{
    /// <summary>
    /// The item that is called.
    /// </summary>
    [JsonPropertyName("to")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CallHierarchyItem To { get; set; }

    /// <summary>
    /// The range at which this item is called. This is the range relative to the caller, e.g the item
    /// passed to {@link CallHierarchyItemProvider.provideCallHierarchyOutgoingCalls `provideCallHierarchyOutgoingCalls`}
    /// and not {@link CallHierarchyOutgoingCall.to `this.to`}.
    /// </summary>
    [JsonPropertyName("fromRanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Range> FromRanges { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokens
{
    /// <summary>
    /// An optional result id. If provided and clients support delta updating
    /// the client will include the result id in the next semantic token request.
    /// A server can then instead of computing all semantic tokens again simply
    /// send a delta.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; set; }

    /// <summary>
    /// The actual tokens.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.UInt32> Data { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensPartialResult
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.UInt32> Data { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensRegistrationOptions : ITextDocumentRegistrationOptions, ISemanticTokensOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The legend used by the server
    /// </summary>
    [JsonPropertyName("legend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SemanticTokensLegend Legend { get; set; }

    /// <summary>
    /// Server supports providing semantic tokens for a specific range
    /// of a document.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.RangeOptions>? Range { get; set; }

    /// <summary>
    /// Server supports providing semantic tokens for a full document.
    /// </summary>
    [JsonPropertyName("full")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.FullOptions>? Full { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensOptions : ISemanticTokensOptions, IWorkDoneProgressOptions
{
    public sealed class RangeOptions
    {
    }

    public sealed class FullOptions
    {
        /// <summary>
        /// The server supports deltas for full documents.
        /// </summary>
        [JsonPropertyName("delta")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? Delta { get; set; }
    }

    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// The legend used by the server
    /// </summary>
    [JsonPropertyName("legend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SemanticTokensLegend Legend { get; set; }

    /// <summary>
    /// Server supports providing semantic tokens for a specific range
    /// of a document.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.RangeOptions>? Range { get; set; }

    /// <summary>
    /// Server supports providing semantic tokens for a full document.
    /// </summary>
    [JsonPropertyName("full")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.FullOptions>? Full { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public interface ISemanticTokensOptions
{
    /// <summary>
    /// The legend used by the server
    /// </summary>
    [JsonPropertyName("legend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public SemanticTokensLegend Legend { get; }

    /// <summary>
    /// Server supports providing semantic tokens for a specific range
    /// of a document.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.RangeOptions>? Range { get; }

    /// <summary>
    /// Server supports providing semantic tokens for a full document.
    /// </summary>
    [JsonPropertyName("full")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, SemanticTokensOptions.FullOptions>? Full { get; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensLegend
{
    /// <summary>
    /// The token types a server uses.
    /// </summary>
    [JsonPropertyName("tokenTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> TokenTypes { get; set; }

    /// <summary>
    /// The token modifiers a server uses.
    /// </summary>
    [JsonPropertyName("tokenModifiers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> TokenModifiers { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensDeltaParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The result id of a previous response. The result Id can either point to a full response
    /// or a delta response depending on what was received last.
    /// </summary>
    [JsonPropertyName("previousResultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String PreviousResultId { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensDelta
{
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; set; }

    /// <summary>
    /// The semantic token edits to transform a previous result into a new result.
    /// </summary>
    [JsonPropertyName("edits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<SemanticTokensEdit> Edits { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensEdit
{
    /// <summary>
    /// The start offset of the edit.
    /// </summary>
    [JsonPropertyName("start")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 Start { get; set; }

    /// <summary>
    /// The count of elements to remove.
    /// </summary>
    [JsonPropertyName("deleteCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 DeleteCount { get; set; }

    /// <summary>
    /// The elements to insert.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.UInt32>? Data { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensDeltaPartialResult
{
    [JsonPropertyName("edits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<SemanticTokensEdit> Edits { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensRangeParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The range the semantic tokens are requested for.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }
}

/// <summary>
/// Params to show a resource in the UI.
/// 
/// @since 3.16.0
/// </summary>
public sealed class ShowDocumentParams
{
    /// <summary>
    /// The uri to show.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Uri { get; set; }

    /// <summary>
    /// Indicates to show the resource in an external program.
    /// To show, for example, `https://code.visualstudio.com/`
    /// in the default WEB browser set `external` to `true`.
    /// </summary>
    [JsonPropertyName("external")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? External { get; set; }

    /// <summary>
    /// An optional property to indicate whether the editor
    /// showing the document should take focus or not.
    /// Clients might ignore this property if an external
    /// program is started.
    /// </summary>
    [JsonPropertyName("takeFocus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? TakeFocus { get; set; }

    /// <summary>
    /// An optional selection range if the document is a text
    /// document. Clients might ignore the property if an
    /// external program is started or the file is not a text
    /// file.
    /// </summary>
    [JsonPropertyName("selection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Range? Selection { get; set; }
}

/// <summary>
/// The result of a showDocument request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class ShowDocumentResult
{
    /// <summary>
    /// A boolean indicating if the show was successful.
    /// </summary>
    [JsonPropertyName("success")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean Success { get; set; }
}

public sealed class LinkedEditingRangeParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

/// <summary>
/// The result of a linked editing range request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class LinkedEditingRanges
{
    /// <summary>
    /// A list of ranges that can be edited together. The ranges must have
    /// identical length and contain identical text content. The ranges cannot overlap.
    /// </summary>
    [JsonPropertyName("ranges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Range> Ranges { get; set; }

    /// <summary>
    /// An optional word pattern (regular expression) that describes valid contents for
    /// the given ranges. If no pattern is provided, the client configuration's word
    /// pattern will be used.
    /// </summary>
    [JsonPropertyName("wordPattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? WordPattern { get; set; }
}

public sealed class LinkedEditingRangeRegistrationOptions : ITextDocumentRegistrationOptions, ILinkedEditingRangeOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

public sealed class LinkedEditingRangeOptions : ILinkedEditingRangeOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface ILinkedEditingRangeOptions
{
}

/// <summary>
/// The parameters sent in notifications/requests for user-initiated creation of
/// files.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CreateFilesParams
{
    /// <summary>
    /// An array of all files/folders created in this operation.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<FileCreate> Files { get; set; }
}

/// <summary>
/// Represents information on a file/folder create.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileCreate
{
    /// <summary>
    /// A file:// URI for the location of the file/folder being created.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Uri { get; set; }
}

/// <summary>
/// A workspace edit represents changes to many resources managed in the workspace. The edit
/// should either provide `changes` or `documentChanges`. If documentChanges are present
/// they are preferred over `changes` if the client can handle versioned document edits.
/// 
/// Since version 3.13.0 a workspace edit can contain resource operations as well. If resource
/// operations are present clients need to execute the operations in the order in which they
/// are provided. So a workspace edit for example can consist of the following two changes:
/// (1) a create file a.txt and (2) a text document edit which insert text into file a.txt.
/// 
/// An invalid sequence (e.g. (1) delete file a.txt and (2) insert text into file a.txt) will
/// cause failure of the operation. How the client recovers from the failure is described by
/// the client capability: `workspace.workspaceEdit.failureHandling`
/// </summary>
public sealed class WorkspaceEdit
{
    /// <summary>
    /// Holds changes to existing resources.
    /// </summary>
    [JsonPropertyName("changes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<Draco.Lsp.Model.DocumentUri, IList<ITextEdit>>? Changes { get; set; }

    /// <summary>
    /// Depending on the client capability `workspace.workspaceEdit.resourceOperations` document changes
    /// are either an array of `TextDocumentEdit`s to express changes to n different text documents
    /// where each text document edit addresses a specific version of a text document. Or it can contain
    /// above `TextDocumentEdit`s mixed with create, rename and delete file / folder operations.
    /// 
    /// Whether a client supports versioned document edits is expressed via
    /// `workspace.workspaceEdit.documentChanges` client capability.
    /// 
    /// If a client neither supports `documentChanges` nor `workspace.workspaceEdit.resourceOperations` then
    /// only plain `TextEdit`s using the `changes` property are supported.
    /// </summary>
    [JsonPropertyName("documentChanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<OneOf<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>? DocumentChanges { get; set; }

    /// <summary>
    /// A map of change annotations that can be referenced in `AnnotatedTextEdit`s or create, rename and
    /// delete file / folder operations.
    /// 
    /// Whether clients honor this property depends on the client capability `workspace.changeAnnotationSupport`.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("changeAnnotations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<System.String, ChangeAnnotation>? ChangeAnnotations { get; set; }
}

/// <summary>
/// Describes textual changes on a text document. A TextDocumentEdit describes all changes
/// on a document version Si and after they are applied move the document to version Si+1.
/// So the creator of a TextDocumentEdit doesn't need to sort the array of edits or do any
/// kind of ordering. However the edits must be non overlapping.
/// </summary>
public sealed class TextDocumentEdit
{
    /// <summary>
    /// The text document to change.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OptionalVersionedTextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The edits to be applied.
    /// 
    /// @since 3.16.0 - support for AnnotatedTextEdit. This is guarded using a
    /// client capability.
    /// </summary>
    [JsonPropertyName("edits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<OneOf<ITextEdit, AnnotatedTextEdit>> Edits { get; set; }
}

/// <summary>
/// A text document identifier to optionally denote a specific version of a text document.
/// </summary>
public sealed class OptionalVersionedTextDocumentIdentifier : ITextDocumentIdentifier
{
    /// <summary>
    /// The text document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The version number of this document. If a versioned text document identifier
    /// is sent from the server to the client and the file is not open in the editor
    /// (the server has not received an open notification before) the server can send
    /// `null` to indicate that the version is unknown and the content on disk is the
    /// truth (as specified with document content ownership).
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32? Version { get; set; }
}

/// <summary>
/// A special text edit with an additional change annotation.
/// 
/// @since 3.16.0.
/// </summary>
public sealed class AnnotatedTextEdit : ITextEdit
{
    /// <summary>
    /// The range of the text document to be manipulated. To insert
    /// text into a document create a range where start === end.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The string to be inserted. For delete operations use an
    /// empty string.
    /// </summary>
    [JsonPropertyName("newText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NewText { get; set; }

    /// <summary>
    /// The actual identifier of the change annotation
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String AnnotationId { get; set; }
}

/// <summary>
/// Create file operation.
/// </summary>
public sealed class CreateFile : IResourceOperation
{
    /// <summary>
    /// A create
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "create";

    /// <summary>
    /// An optional annotation identifier describing the operation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? AnnotationId { get; set; }

    /// <summary>
    /// The resource to create.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// Additional options
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CreateFileOptions? Options { get; set; }
}

/// <summary>
/// A generic resource operation.
/// </summary>
public sealed class ResourceOperation : IResourceOperation
{
    /// <summary>
    /// The resource operation kind.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Kind { get; set; }

    /// <summary>
    /// An optional annotation identifier describing the operation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? AnnotationId { get; set; }
}

/// <summary>
/// A generic resource operation.
/// </summary>
public interface IResourceOperation
{
    /// <summary>
    /// The resource operation kind.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind { get; }

    /// <summary>
    /// An optional annotation identifier describing the operation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? AnnotationId { get; }
}

/// <summary>
/// Options to create a file.
/// </summary>
public sealed class CreateFileOptions
{
    /// <summary>
    /// Overwrite existing file. Overwrite wins over `ignoreIfExists`
    /// </summary>
    [JsonPropertyName("overwrite")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Overwrite { get; set; }

    /// <summary>
    /// Ignore if exists.
    /// </summary>
    [JsonPropertyName("ignoreIfExists")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IgnoreIfExists { get; set; }
}

/// <summary>
/// Rename file operation
/// </summary>
public sealed class RenameFile : IResourceOperation
{
    /// <summary>
    /// A rename
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "rename";

    /// <summary>
    /// An optional annotation identifier describing the operation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? AnnotationId { get; set; }

    /// <summary>
    /// The old (existing) location.
    /// </summary>
    [JsonPropertyName("oldUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri OldUri { get; set; }

    /// <summary>
    /// The new location.
    /// </summary>
    [JsonPropertyName("newUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri NewUri { get; set; }

    /// <summary>
    /// Rename options.
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RenameFileOptions? Options { get; set; }
}

/// <summary>
/// Rename file options
/// </summary>
public sealed class RenameFileOptions
{
    /// <summary>
    /// Overwrite target if existing. Overwrite wins over `ignoreIfExists`
    /// </summary>
    [JsonPropertyName("overwrite")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Overwrite { get; set; }

    /// <summary>
    /// Ignores if target exists.
    /// </summary>
    [JsonPropertyName("ignoreIfExists")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IgnoreIfExists { get; set; }
}

/// <summary>
/// Delete file operation
/// </summary>
public sealed class DeleteFile : IResourceOperation
{
    /// <summary>
    /// A delete
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "delete";

    /// <summary>
    /// An optional annotation identifier describing the operation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("annotationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? AnnotationId { get; set; }

    /// <summary>
    /// The file to delete.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// Delete options.
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DeleteFileOptions? Options { get; set; }
}

/// <summary>
/// Delete file options
/// </summary>
public sealed class DeleteFileOptions
{
    /// <summary>
    /// Delete the content recursively if a folder is denoted.
    /// </summary>
    [JsonPropertyName("recursive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Recursive { get; set; }

    /// <summary>
    /// Ignore the operation if the file doesn't exist.
    /// </summary>
    [JsonPropertyName("ignoreIfNotExists")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IgnoreIfNotExists { get; set; }
}

/// <summary>
/// Additional information that describes document changes.
/// 
/// @since 3.16.0
/// </summary>
public sealed class ChangeAnnotation
{
    /// <summary>
    /// A human-readable string describing the actual change. The string
    /// is rendered prominent in the user interface.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Label { get; set; }

    /// <summary>
    /// A flag which indicates that user confirmation is needed
    /// before applying the change.
    /// </summary>
    [JsonPropertyName("needsConfirmation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? NeedsConfirmation { get; set; }

    /// <summary>
    /// A human-readable string which is rendered less prominent in
    /// the user interface.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Description { get; set; }
}

/// <summary>
/// The options to register for file operations.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationRegistrationOptions
{
    /// <summary>
    /// The actual filters.
    /// </summary>
    [JsonPropertyName("filters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<FileOperationFilter> Filters { get; set; }
}

/// <summary>
/// A filter to describe in which file operation requests or notifications
/// the server is interested in receiving.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationFilter
{
    /// <summary>
    /// A Uri scheme like `file` or `untitled`.
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Scheme { get; set; }

    /// <summary>
    /// The actual file operation pattern.
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required FileOperationPattern Pattern { get; set; }
}

/// <summary>
/// A pattern to describe in which file operation requests or notifications
/// the server is interested in receiving.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationPattern
{
    /// <summary>
    /// The glob pattern to match. Glob patterns can have the following syntax:
    /// - `*` to match one or more characters in a path segment
    /// - `?` to match on one character in a path segment
    /// - `**` to match any number of path segments, including none
    /// - `{}` to group sub patterns into an OR expression. (e.g. `**​/*.{ts,js}` matches all TypeScript and JavaScript files)
    /// - `[]` to declare a range of characters to match in a path segment (e.g., `example.[0-9]` to match on `example.0`, `example.1`, …)
    /// - `[!...]` to negate a range of characters to match in a path segment (e.g., `example.[!0-9]` to match on `example.a`, `example.b`, but not `example.0`)
    /// </summary>
    [JsonPropertyName("glob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Glob { get; set; }

    /// <summary>
    /// Whether to match files or folders with this pattern.
    /// 
    /// Matches both if undefined.
    /// </summary>
    [JsonPropertyName("matches")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationPatternKind? Matches { get; set; }

    /// <summary>
    /// Additional options used during matching.
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationPatternOptions? Options { get; set; }
}

/// <summary>
/// A pattern kind describing if a glob pattern matches a file a folder or
/// both.
/// 
/// @since 3.16.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum FileOperationPatternKind
{
    /// <summary>
    /// The pattern matches a file only.
    /// </summary>
    [EnumMember(Value = "file")]
    File,
    /// <summary>
    /// The pattern matches a folder only.
    /// </summary>
    [EnumMember(Value = "folder")]
    Folder,
}

/// <summary>
/// Matching options for the file operation pattern.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationPatternOptions
{
    /// <summary>
    /// The pattern should be matched ignoring casing.
    /// </summary>
    [JsonPropertyName("ignoreCase")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IgnoreCase { get; set; }
}

/// <summary>
/// The parameters sent in notifications/requests for user-initiated renames of
/// files.
/// 
/// @since 3.16.0
/// </summary>
public sealed class RenameFilesParams
{
    /// <summary>
    /// An array of all files/folders renamed in this operation. When a folder is renamed, only
    /// the folder will be included, and not its children.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<FileRename> Files { get; set; }
}

/// <summary>
/// Represents information on a file/folder rename.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileRename
{
    /// <summary>
    /// A file:// URI for the original location of the file/folder being renamed.
    /// </summary>
    [JsonPropertyName("oldUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String OldUri { get; set; }

    /// <summary>
    /// A file:// URI for the new location of the file/folder being renamed.
    /// </summary>
    [JsonPropertyName("newUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NewUri { get; set; }
}

/// <summary>
/// The parameters sent in notifications/requests for user-initiated deletes of
/// files.
/// 
/// @since 3.16.0
/// </summary>
public sealed class DeleteFilesParams
{
    /// <summary>
    /// An array of all files/folders deleted in this operation.
    /// </summary>
    [JsonPropertyName("files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Draco.Lsp.Model.FileDelete> Files { get; set; }
}

public sealed class MonikerParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

/// <summary>
/// Moniker definition to match LSIF 0.5 moniker definition.
/// 
/// @since 3.16.0
/// </summary>
public sealed class Moniker
{
    /// <summary>
    /// The scheme of the moniker. For example tsc or .Net
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Scheme { get; set; }

    /// <summary>
    /// The identifier of the moniker. The value is opaque in LSIF however
    /// schema owners are allowed to define the structure if they want.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Identifier { get; set; }

    /// <summary>
    /// The scope in which the moniker is unique
    /// </summary>
    [JsonPropertyName("unique")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required UniquenessLevel Unique { get; set; }

    /// <summary>
    /// The moniker kind if known.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MonikerKind? Kind { get; set; }
}

/// <summary>
/// Moniker uniqueness level to define scope of the moniker.
/// 
/// @since 3.16.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum UniquenessLevel
{
    /// <summary>
    /// The moniker is only unique inside a document
    /// </summary>
    [EnumMember(Value = "document")]
    Document,
    /// <summary>
    /// The moniker is unique inside a project for which a dump got created
    /// </summary>
    [EnumMember(Value = "project")]
    Project,
    /// <summary>
    /// The moniker is unique inside the group to which a project belongs
    /// </summary>
    [EnumMember(Value = "group")]
    Group,
    /// <summary>
    /// The moniker is unique inside the moniker scheme.
    /// </summary>
    [EnumMember(Value = "scheme")]
    Scheme,
    /// <summary>
    /// The moniker is globally unique
    /// </summary>
    [EnumMember(Value = "global")]
    Global,
}

/// <summary>
/// The moniker kind.
/// 
/// @since 3.16.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum MonikerKind
{
    /// <summary>
    /// The moniker represent a symbol that is imported into a project
    /// </summary>
    [EnumMember(Value = "import")]
    Import,
    /// <summary>
    /// The moniker represents a symbol that is exported from a project
    /// </summary>
    [EnumMember(Value = "export")]
    Export,
    /// <summary>
    /// The moniker represents a symbol that is local to a project (e.g. a local
    /// variable of a function, a class not visible outside the project, ...)
    /// </summary>
    [EnumMember(Value = "local")]
    Local,
}

public sealed class MonikerRegistrationOptions : ITextDocumentRegistrationOptions, IMonikerOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

public sealed class MonikerOptions : IMonikerOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

public interface IMonikerOptions
{
}

/// <summary>
/// The parameter of a `textDocument/prepareTypeHierarchy` request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchyPrepareParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

/// <summary>
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchyItem
{
    /// <summary>
    /// The name of this item.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// The kind of this item.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this item.
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// More detail for this item, e.g. the signature of a function.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Detail { get; set; }

    /// <summary>
    /// The resource identifier of this item.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The range enclosing this symbol not including leading/trailing whitespace
    /// but everything else, e.g. comments and code.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The range that should be selected and revealed when this symbol is being
    /// picked, e.g. the name of a function. Must be contained by the
    /// {@link TypeHierarchyItem.range `range`}.
    /// </summary>
    [JsonPropertyName("selectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range SelectionRange { get; set; }

    /// <summary>
    /// A data entry field that is preserved between a type hierarchy prepare and
    /// supertypes or subtypes requests. It could also be used to identify the
    /// type hierarchy in the server, helping improve the performance on
    /// resolving supertypes and subtypes.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Type hierarchy options used during static or dynamic registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchyRegistrationOptions : ITextDocumentRegistrationOptions, ITypeHierarchyOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Type hierarchy options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchyOptions : ITypeHierarchyOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Type hierarchy options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public interface ITypeHierarchyOptions
{
}

/// <summary>
/// The parameter of a `typeHierarchy/supertypes` request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchySupertypesParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    [JsonPropertyName("item")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TypeHierarchyItem Item { get; set; }
}

/// <summary>
/// The parameter of a `typeHierarchy/subtypes` request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchySubtypesParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    [JsonPropertyName("item")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TypeHierarchyItem Item { get; set; }
}

/// <summary>
/// A parameter literal used in inline value requests.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The document range for which inline values should be computed.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// Additional information about the context in which inline values were
    /// requested.
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required InlineValueContext Context { get; set; }
}

/// <summary>
/// @since 3.17.0
/// </summary>
public sealed class InlineValueContext
{
    /// <summary>
    /// The stack frame (as a DAP Id) where the execution has stopped.
    /// </summary>
    [JsonPropertyName("frameId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32 FrameId { get; set; }

    /// <summary>
    /// The document range where execution has stopped.
    /// Typically the end position of the range denotes the line where the inline values are shown.
    /// </summary>
    [JsonPropertyName("stoppedLocation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range StoppedLocation { get; set; }
}

/// <summary>
/// Inline value options used during static or dynamic registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueRegistrationOptions : IInlineValueOptions, ITextDocumentRegistrationOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Inline value options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueOptions : IInlineValueOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Inline value options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public interface IInlineValueOptions
{
}

/// <summary>
/// A parameter literal used in inlay hint requests.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The document range for which inlay hints should be computed.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }
}

/// <summary>
/// Inlay hint information.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHint
{
    /// <summary>
    /// The position of this hint.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// The label of this hint. A human readable string or an array of
    /// InlayHintLabelPart label parts.
    /// 
    /// *Note* that neither the string nor the label part can be empty.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.String, IList<InlayHintLabelPart>> Label { get; set; }

    /// <summary>
    /// The kind of this hint. Can be omitted in which case the client
    /// should fall back to a reasonable default.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlayHintKind? Kind { get; set; }

    /// <summary>
    /// Optional text edits that are performed when accepting this inlay hint.
    /// 
    /// *Note* that edits are expected to change the document so that the inlay
    /// hint (or its nearest variant) is now part of the document and the inlay
    /// hint itself is now obsolete.
    /// </summary>
    [JsonPropertyName("textEdits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ITextEdit>? TextEdits { get; set; }

    /// <summary>
    /// The tooltip text when you hover over this item.
    /// </summary>
    [JsonPropertyName("tooltip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, MarkupContent>? Tooltip { get; set; }

    /// <summary>
    /// Render padding before the hint.
    /// 
    /// Note: Padding should use the editor's background color, not the
    /// background color of the hint itself. That means padding can be used
    /// to visually align/separate an inlay hint.
    /// </summary>
    [JsonPropertyName("paddingLeft")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PaddingLeft { get; set; }

    /// <summary>
    /// Render padding after the hint.
    /// 
    /// Note: Padding should use the editor's background color, not the
    /// background color of the hint itself. That means padding can be used
    /// to visually align/separate an inlay hint.
    /// </summary>
    [JsonPropertyName("paddingRight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PaddingRight { get; set; }

    /// <summary>
    /// A data entry field that is preserved on an inlay hint between
    /// a `textDocument/inlayHint` and a `inlayHint/resolve` request.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// An inlay hint label part allows for interactive and composite labels
/// of inlay hints.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintLabelPart
{
    /// <summary>
    /// The value of this label part.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Value { get; set; }

    /// <summary>
    /// The tooltip text when you hover over this label part. Depending on
    /// the client capability `inlayHint.resolveSupport` clients might resolve
    /// this property late using the resolve request.
    /// </summary>
    [JsonPropertyName("tooltip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, MarkupContent>? Tooltip { get; set; }

    /// <summary>
    /// An optional source code location that represents this
    /// label part.
    /// 
    /// The editor will use this location for the hover and for code navigation
    /// features: This part will become a clickable link that resolves to the
    /// definition of the symbol at the given location (not necessarily the
    /// location itself), it shows the hover that shows at the given location,
    /// and it shows a context menu with further code navigation commands.
    /// 
    /// Depending on the client capability `inlayHint.resolveSupport` clients
    /// might resolve this property late using the resolve request.
    /// </summary>
    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Location? Location { get; set; }

    /// <summary>
    /// An optional command for this label part.
    /// 
    /// Depending on the client capability `inlayHint.resolveSupport` clients
    /// might resolve this property late using the resolve request.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Command? Command { get; set; }
}

/// <summary>
/// A `MarkupContent` literal represents a string value which content is interpreted base on its
/// kind flag. Currently the protocol supports `plaintext` and `markdown` as markup kinds.
/// 
/// If the kind is `markdown` then the value can contain fenced code blocks like in GitHub issues.
/// See https://help.github.com/articles/creating-and-highlighting-code-blocks/#syntax-highlighting
/// 
/// Here is an example how such a string can be constructed using JavaScript / TypeScript:
/// ```ts
/// let markdown: MarkdownContent = {
///  kind: MarkupKind.Markdown,
///  value: [
///    '# Header',
///    'Some text',
///    '```typescript',
///    'someCode();',
///    '```'
///  ].join('\n')
/// };
/// ```
/// 
/// *Please Note* that clients might sanitize the return markdown. A client could decide to
/// remove HTML from the markdown to avoid script execution.
/// </summary>
public sealed class MarkupContent
{
    /// <summary>
    /// The type of the Markup
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required MarkupKind Kind { get; set; }

    /// <summary>
    /// The content itself
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Value { get; set; }
}

/// <summary>
/// Describes the content type that a client supports in various
/// result literals like `Hover`, `ParameterInfo` or `CompletionItem`.
/// 
/// Please note that `MarkupKinds` must not start with a `$`. This kinds
/// are reserved for internal usage.
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum MarkupKind
{
    /// <summary>
    /// Plain text is supported as a content format
    /// </summary>
    [EnumMember(Value = "plaintext")]
    PlainText,
    /// <summary>
    /// Markdown is supported as a content format
    /// </summary>
    [EnumMember(Value = "markdown")]
    Markdown,
}

/// <summary>
/// Represents a reference to a command. Provides a title which
/// will be used to represent a command in the UI and, optionally,
/// an array of arguments which will be passed to the command handler
/// function when invoked.
/// </summary>
public sealed class Command
{
    /// <summary>
    /// Title of the command, like `save`.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Title { get; set; }

    /// <summary>
    /// The identifier of the actual command handler.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Command_ { get; set; }

    /// <summary>
    /// Arguments that the command handler should be
    /// invoked with.
    /// </summary>
    [JsonPropertyName("arguments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.Text.Json.JsonElement>? Arguments { get; set; }
}

/// <summary>
/// Inlay hint kinds.
/// 
/// @since 3.17.0
/// </summary>
public enum InlayHintKind
{
    /// <summary>
    /// An inlay hint that for a type annotation.
    /// </summary>
    Type = 1,
    /// <summary>
    /// An inlay hint that is for a parameter.
    /// </summary>
    Parameter = 2,
}

/// <summary>
/// Inlay hint options used during static or dynamic registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintRegistrationOptions : IInlayHintOptions, ITextDocumentRegistrationOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// The server provides support to resolve additional
    /// information for an inlay hint item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }

    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Inlay hint options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintOptions : IInlayHintOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for an inlay hint item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// Inlay hint options used during static registration.
/// 
/// @since 3.17.0
/// </summary>
public interface IInlayHintOptions
{
    /// <summary>
    /// The server provides support to resolve additional
    /// information for an inlay hint item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }
}

/// <summary>
/// Parameters of the document diagnostic request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DocumentDiagnosticParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The additional identifier  provided during registration.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Identifier { get; set; }

    /// <summary>
    /// The result id of a previous response if provided.
    /// </summary>
    [JsonPropertyName("previousResultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? PreviousResultId { get; set; }
}

/// <summary>
/// A partial result for a document diagnostic report.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DocumentDiagnosticReportPartialResult
{
    [JsonPropertyName("relatedDocuments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IDictionary<Draco.Lsp.Model.DocumentUri, OneOf<IFullDocumentDiagnosticReport, IUnchangedDocumentDiagnosticReport>> RelatedDocuments { get; set; }
}

/// <summary>
/// A diagnostic report with a full set of problems.
/// 
/// @since 3.17.0
/// </summary>
public sealed class FullDocumentDiagnosticReport : IFullDocumentDiagnosticReport
{
    /// <summary>
    /// A full document diagnostic report.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "full";

    /// <summary>
    /// An optional result id. If provided it will
    /// be sent on the next diagnostic request for the
    /// same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; set; }

    /// <summary>
    /// The actual items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Diagnostic> Items { get; set; }
}

/// <summary>
/// A diagnostic report with a full set of problems.
/// 
/// @since 3.17.0
/// </summary>
public interface IFullDocumentDiagnosticReport
{
    /// <summary>
    /// A full document diagnostic report.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "full";

    /// <summary>
    /// An optional result id. If provided it will
    /// be sent on the next diagnostic request for the
    /// same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; }

    /// <summary>
    /// The actual items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public IList<Diagnostic> Items { get; }
}

/// <summary>
/// Represents a diagnostic, such as a compiler error or warning. Diagnostic objects
/// are only valid in the scope of a resource.
/// </summary>
public sealed class Diagnostic
{
    /// <summary>
    /// The range at which the message applies
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The diagnostic's severity. Can be omitted. If omitted it is up to the
    /// client to interpret diagnostics as error, warning, info or hint.
    /// </summary>
    [JsonPropertyName("severity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiagnosticSeverity? Severity { get; set; }

    /// <summary>
    /// The diagnostic's code, which usually appear in the user interface.
    /// </summary>
    [JsonPropertyName("code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? Code { get; set; }

    /// <summary>
    /// An optional property to describe the error code.
    /// Requires the code field (above) to be present/not null.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("codeDescription")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeDescription? CodeDescription { get; set; }

    /// <summary>
    /// A human-readable string describing the source of this
    /// diagnostic, e.g. 'typescript' or 'super lint'. It usually
    /// appears in the user interface.
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Source { get; set; }

    /// <summary>
    /// The diagnostic's message. It usually appears in the user interface
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }

    /// <summary>
    /// Additional metadata about the diagnostic.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<DiagnosticTag>? Tags { get; set; }

    /// <summary>
    /// An array of related diagnostic information, e.g. when symbol-names within
    /// a scope collide all definitions can be marked via this property.
    /// </summary>
    [JsonPropertyName("relatedInformation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<DiagnosticRelatedInformation>? RelatedInformation { get; set; }

    /// <summary>
    /// A data entry field that is preserved between a `textDocument/publishDiagnostics`
    /// notification and `textDocument/codeAction` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// The diagnostic's severity.
/// </summary>
public enum DiagnosticSeverity
{
    /// <summary>
    /// Reports an error.
    /// </summary>
    Error = 1,
    /// <summary>
    /// Reports a warning.
    /// </summary>
    Warning = 2,
    /// <summary>
    /// Reports an information.
    /// </summary>
    Information = 3,
    /// <summary>
    /// Reports a hint.
    /// </summary>
    Hint = 4,
}

/// <summary>
/// Structure to capture a description for an error code.
/// 
/// @since 3.16.0
/// </summary>
public sealed class CodeDescription
{
    /// <summary>
    /// An URI to open with more information about the diagnostic error.
    /// </summary>
    [JsonPropertyName("href")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Href { get; set; }
}

/// <summary>
/// The diagnostic tags.
/// 
/// @since 3.15.0
/// </summary>
public enum DiagnosticTag
{
    /// <summary>
    /// Unused or unnecessary code.
    /// 
    /// Clients are allowed to render diagnostics with this tag faded out instead of having
    /// an error squiggle.
    /// </summary>
    Unnecessary = 1,
    /// <summary>
    /// Deprecated or obsolete code.
    /// 
    /// Clients are allowed to rendered diagnostics with this tag strike through.
    /// </summary>
    Deprecated = 2,
}

/// <summary>
/// Represents a related message and source code location for a diagnostic. This should be
/// used to point to code locations that cause or related to a diagnostics, e.g when duplicating
/// a symbol in a scope.
/// </summary>
public sealed class DiagnosticRelatedInformation
{
    /// <summary>
    /// The location of this related diagnostic information.
    /// </summary>
    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Location Location { get; set; }

    /// <summary>
    /// The message of this related diagnostic information.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }
}

/// <summary>
/// A diagnostic report indicating that the last returned
/// report is still accurate.
/// 
/// @since 3.17.0
/// </summary>
public sealed class UnchangedDocumentDiagnosticReport : IUnchangedDocumentDiagnosticReport
{
    /// <summary>
    /// A document diagnostic report indicating
    /// no changes to the last result. A server can
    /// only return `unchanged` if result ids are
    /// provided.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "unchanged";

    /// <summary>
    /// A result id which will be sent on the next
    /// diagnostic request for the same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String ResultId { get; set; }
}

/// <summary>
/// A diagnostic report indicating that the last returned
/// report is still accurate.
/// 
/// @since 3.17.0
/// </summary>
public interface IUnchangedDocumentDiagnosticReport
{
    /// <summary>
    /// A document diagnostic report indicating
    /// no changes to the last result. A server can
    /// only return `unchanged` if result ids are
    /// provided.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "unchanged";

    /// <summary>
    /// A result id which will be sent on the next
    /// diagnostic request for the same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String ResultId { get; }
}

/// <summary>
/// Cancellation data returned from a diagnostic request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DiagnosticServerCancellationData
{
    [JsonPropertyName("retriggerRequest")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean RetriggerRequest { get; set; }
}

/// <summary>
/// Diagnostic registration options.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DiagnosticRegistrationOptions : ITextDocumentRegistrationOptions, IDiagnosticOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// An optional identifier under which the diagnostics are
    /// managed by the client.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Identifier { get; set; }

    /// <summary>
    /// Whether the language has inter file dependencies meaning that
    /// editing code in one file can result in a different diagnostic
    /// set in another file. Inter file dependencies are common for
    /// most programming languages and typically uncommon for linters.
    /// </summary>
    [JsonPropertyName("interFileDependencies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean InterFileDependencies { get; set; }

    /// <summary>
    /// The server provides support for workspace diagnostics as well.
    /// </summary>
    [JsonPropertyName("workspaceDiagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean WorkspaceDiagnostics { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Diagnostic options.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DiagnosticOptions : IDiagnosticOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// An optional identifier under which the diagnostics are
    /// managed by the client.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Identifier { get; set; }

    /// <summary>
    /// Whether the language has inter file dependencies meaning that
    /// editing code in one file can result in a different diagnostic
    /// set in another file. Inter file dependencies are common for
    /// most programming languages and typically uncommon for linters.
    /// </summary>
    [JsonPropertyName("interFileDependencies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean InterFileDependencies { get; set; }

    /// <summary>
    /// The server provides support for workspace diagnostics as well.
    /// </summary>
    [JsonPropertyName("workspaceDiagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean WorkspaceDiagnostics { get; set; }
}

/// <summary>
/// Diagnostic options.
/// 
/// @since 3.17.0
/// </summary>
public interface IDiagnosticOptions
{
    /// <summary>
    /// An optional identifier under which the diagnostics are
    /// managed by the client.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Identifier { get; }

    /// <summary>
    /// Whether the language has inter file dependencies meaning that
    /// editing code in one file can result in a different diagnostic
    /// set in another file. Inter file dependencies are common for
    /// most programming languages and typically uncommon for linters.
    /// </summary>
    [JsonPropertyName("interFileDependencies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.Boolean InterFileDependencies { get; }

    /// <summary>
    /// The server provides support for workspace diagnostics as well.
    /// </summary>
    [JsonPropertyName("workspaceDiagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.Boolean WorkspaceDiagnostics { get; }
}

/// <summary>
/// Parameters of the workspace diagnostic request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceDiagnosticParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The additional identifier provided during registration.
    /// </summary>
    [JsonPropertyName("identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Identifier { get; set; }

    /// <summary>
    /// The currently known diagnostic reports with their
    /// previous result ids.
    /// </summary>
    [JsonPropertyName("previousResultIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<PreviousResultId> PreviousResultIds { get; set; }
}

/// <summary>
/// A previous result id in a workspace pull request.
/// 
/// @since 3.17.0
/// </summary>
public sealed class PreviousResultId
{
    /// <summary>
    /// The URI for which the client knowns a
    /// result id.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The value of the previous result id.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Value { get; set; }
}

/// <summary>
/// A workspace diagnostic report.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceDiagnosticReport
{
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<OneOf<WorkspaceFullDocumentDiagnosticReport, WorkspaceUnchangedDocumentDiagnosticReport>> Items { get; set; }
}

/// <summary>
/// A full document diagnostic report for a workspace diagnostic result.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceFullDocumentDiagnosticReport : IFullDocumentDiagnosticReport
{
    /// <summary>
    /// A full document diagnostic report.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "full";

    /// <summary>
    /// An optional result id. If provided it will
    /// be sent on the next diagnostic request for the
    /// same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; set; }

    /// <summary>
    /// The actual items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Diagnostic> Items { get; set; }

    /// <summary>
    /// The URI for which diagnostic information is reported.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The version number for which the diagnostics are reported.
    /// If the document is not marked as open `null` can be provided.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32? Version { get; set; }
}

/// <summary>
/// An unchanged document diagnostic report for a workspace diagnostic result.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceUnchangedDocumentDiagnosticReport : IUnchangedDocumentDiagnosticReport
{
    /// <summary>
    /// A document diagnostic report indicating
    /// no changes to the last result. A server can
    /// only return `unchanged` if result ids are
    /// provided.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "unchanged";

    /// <summary>
    /// A result id which will be sent on the next
    /// diagnostic request for the same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String ResultId { get; set; }

    /// <summary>
    /// The URI for which diagnostic information is reported.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The version number for which the diagnostics are reported.
    /// If the document is not marked as open `null` can be provided.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32? Version { get; set; }
}

/// <summary>
/// A partial result for a workspace diagnostic report.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceDiagnosticReportPartialResult
{
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<OneOf<WorkspaceFullDocumentDiagnosticReport, WorkspaceUnchangedDocumentDiagnosticReport>> Items { get; set; }
}

/// <summary>
/// The params sent in an open notebook document notification.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DidOpenNotebookDocumentParams
{
    /// <summary>
    /// The notebook document that got opened.
    /// </summary>
    [JsonPropertyName("notebookDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookDocument NotebookDocument { get; set; }

    /// <summary>
    /// The text documents that represent the content
    /// of a notebook cell.
    /// </summary>
    [JsonPropertyName("cellTextDocuments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<TextDocumentItem> CellTextDocuments { get; set; }
}

/// <summary>
/// A notebook document.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocument
{
    /// <summary>
    /// The notebook document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Uri { get; set; }

    /// <summary>
    /// The type of the notebook.
    /// </summary>
    [JsonPropertyName("notebookType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NotebookType { get; set; }

    /// <summary>
    /// The version number of this document (it will increase after each
    /// change, including undo/redo).
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32 Version { get; set; }

    /// <summary>
    /// Additional metadata stored with the notebook
    /// document.
    /// 
    /// Note: should always be an object literal (e.g. LSPObject)
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<System.String, System.Text.Json.JsonElement>? Metadata { get; set; }

    /// <summary>
    /// The cells of a notebook.
    /// </summary>
    [JsonPropertyName("cells")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<NotebookCell> Cells { get; set; }
}

/// <summary>
/// A notebook cell.
/// 
/// A cell's document URI must be unique across ALL notebook
/// cells and can therefore be used to uniquely identify a
/// notebook cell or the cell's text document.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookCell
{
    /// <summary>
    /// The cell's kind
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookCellKind Kind { get; set; }

    /// <summary>
    /// The URI of the cell's text document
    /// content.
    /// </summary>
    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Document { get; set; }

    /// <summary>
    /// Additional metadata stored with the cell.
    /// 
    /// Note: should always be an object literal (e.g. LSPObject)
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<System.String, System.Text.Json.JsonElement>? Metadata { get; set; }

    /// <summary>
    /// Additional execution summary information
    /// if supported by the client.
    /// </summary>
    [JsonPropertyName("executionSummary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ExecutionSummary? ExecutionSummary { get; set; }
}

/// <summary>
/// A notebook cell kind.
/// 
/// @since 3.17.0
/// </summary>
public enum NotebookCellKind
{
    /// <summary>
    /// A markup-cell is formatted source that is used for display.
    /// </summary>
    Markup = 1,
    /// <summary>
    /// A code-cell is source code.
    /// </summary>
    Code = 2,
}

public sealed class ExecutionSummary
{
    /// <summary>
    /// A strict monotonically increasing value
    /// indicating the execution order of a cell
    /// inside a notebook.
    /// </summary>
    [JsonPropertyName("executionOrder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 ExecutionOrder { get; set; }

    /// <summary>
    /// Whether the execution was successful or
    /// not if known by the client.
    /// </summary>
    [JsonPropertyName("success")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Success { get; set; }
}

/// <summary>
/// An item to transfer a text document from the client to the
/// server.
/// </summary>
public sealed class TextDocumentItem
{
    /// <summary>
    /// The text document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The text document's language identifier.
    /// </summary>
    [JsonPropertyName("languageId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String LanguageId { get; set; }

    /// <summary>
    /// The version number of this document (it will increase after each
    /// change, including undo/redo).
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32 Version { get; set; }

    /// <summary>
    /// The content of the opened text document.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Text { get; set; }
}

/// <summary>
/// The params sent in a change notebook document notification.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DidChangeNotebookDocumentParams
{
    /// <summary>
    /// The notebook document that did change. The version number points
    /// to the version after all provided changes have been applied. If
    /// only the text document content of a cell changes the notebook version
    /// doesn't necessarily have to change.
    /// </summary>
    [JsonPropertyName("notebookDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required VersionedNotebookDocumentIdentifier NotebookDocument { get; set; }

    /// <summary>
    /// The actual changes to the notebook document.
    /// 
    /// The changes describe single state changes to the notebook document.
    /// So if there are two changes c1 (at array index 0) and c2 (at array
    /// index 1) for a notebook in state S then c1 moves the notebook from
    /// S to S' and c2 from S' to S''. So c1 is computed on the state S and
    /// c2 is computed on the state S'.
    /// 
    /// To mirror the content of a notebook using change events use the following approach:
    /// - start with the same initial content
    /// - apply the 'notebookDocument/didChange' notifications in the order you receive them.
    /// - apply the `NotebookChangeEvent`s in a single notification in the order
    ///   you receive them.
    /// </summary>
    [JsonPropertyName("change")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookDocumentChangeEvent Change { get; set; }
}

/// <summary>
/// A versioned notebook document identifier.
/// 
/// @since 3.17.0
/// </summary>
public sealed class VersionedNotebookDocumentIdentifier
{
    /// <summary>
    /// The version number of this notebook document.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32 Version { get; set; }

    /// <summary>
    /// The notebook document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Uri { get; set; }
}

/// <summary>
/// A change event for a notebook document.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentChangeEvent
{
    public sealed class CellsEvent
    {
        public sealed class StructureEvent
        {
            /// <summary>
            /// The change to the cell array.
            /// </summary>
            [JsonPropertyName("array")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required NotebookCellArrayChange Array { get; set; }

            /// <summary>
            /// Additional opened cell text documents.
            /// </summary>
            [JsonPropertyName("didOpen")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public IList<TextDocumentItem>? DidOpen { get; set; }

            /// <summary>
            /// Additional closed cell text documents.
            /// </summary>
            [JsonPropertyName("didClose")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public IList<ITextDocumentIdentifier>? DidClose { get; set; }
        }

        public sealed class TextContentEvent
        {
            [JsonPropertyName("document")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required VersionedTextDocumentIdentifier Document { get; set; }

            [JsonPropertyName("changes")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required IList<TextDocumentContentChangeEvent> Changes { get; set; }
        }

        /// <summary>
        /// Changes to the cell structure to add or
        /// remove cells.
        /// </summary>
        [JsonPropertyName("structure")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public NotebookDocumentChangeEvent.CellsEvent.StructureEvent? Structure { get; set; }

        /// <summary>
        /// Changes to notebook cells properties like its
        /// kind, execution summary or metadata.
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<NotebookCell>? Data { get; set; }

        /// <summary>
        /// Changes to the text content of notebook cells.
        /// </summary>
        [JsonPropertyName("textContent")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<NotebookDocumentChangeEvent.CellsEvent.TextContentEvent>? TextContent { get; set; }
    }

    /// <summary>
    /// The changed meta data if any.
    /// 
    /// Note: should always be an object literal (e.g. LSPObject)
    /// </summary>
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<System.String, System.Text.Json.JsonElement>? Metadata { get; set; }

    /// <summary>
    /// Changes to cells
    /// </summary>
    [JsonPropertyName("cells")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NotebookDocumentChangeEvent.CellsEvent? Cells { get; set; }
}

/// <summary>
/// A change describing how to move a `NotebookCell`
/// array from state S to S'.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookCellArrayChange
{
    /// <summary>
    /// The start oftest of the cell that changed.
    /// </summary>
    [JsonPropertyName("start")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 Start { get; set; }

    /// <summary>
    /// The deleted cells
    /// </summary>
    [JsonPropertyName("deleteCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 DeleteCount { get; set; }

    /// <summary>
    /// The new cells, if any
    /// </summary>
    [JsonPropertyName("cells")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<NotebookCell>? Cells { get; set; }
}

/// <summary>
/// A text document identifier to denote a specific version of a text document.
/// </summary>
public sealed class VersionedTextDocumentIdentifier : ITextDocumentIdentifier
{
    /// <summary>
    /// The text document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The version number of this document.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32 Version { get; set; }
}

public sealed class TextDocumentContentChangeEvent
{
    /// <summary>
    /// The new text for the provided range.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Text { get; set; }

    /// <summary>
    /// The range of the document that changed.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Range? Range { get; set; }

    /// <summary>
    /// The optional length of the range that got replaced.
    /// 
    /// @deprecated use range instead.
    /// </summary>
    [JsonPropertyName("rangeLength")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? RangeLength { get; set; }
}

/// <summary>
/// The params sent in a save notebook document notification.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DidSaveNotebookDocumentParams
{
    /// <summary>
    /// The notebook document that got saved.
    /// </summary>
    [JsonPropertyName("notebookDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookDocumentIdentifier NotebookDocument { get; set; }
}

/// <summary>
/// A literal to identify a notebook document in the client.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentIdentifier
{
    /// <summary>
    /// The notebook document's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Uri Uri { get; set; }
}

/// <summary>
/// The params sent in a close notebook document notification.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DidCloseNotebookDocumentParams
{
    /// <summary>
    /// The notebook document that got closed.
    /// </summary>
    [JsonPropertyName("notebookDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookDocumentIdentifier NotebookDocument { get; set; }

    /// <summary>
    /// The text documents that represent the content
    /// of a notebook cell that got closed.
    /// </summary>
    [JsonPropertyName("cellTextDocuments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<ITextDocumentIdentifier> CellTextDocuments { get; set; }
}

public sealed class RegistrationParams
{
    [JsonPropertyName("registrations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Registration> Registrations { get; set; }
}

/// <summary>
/// General parameters to register for a notification or to register a provider.
/// </summary>
public sealed class Registration
{
    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Id { get; set; }

    /// <summary>
    /// The method / capability to register for.
    /// </summary>
    [JsonPropertyName("method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Method { get; set; }

    /// <summary>
    /// Options necessary for the registration.
    /// </summary>
    [JsonPropertyName("registerOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? RegisterOptions { get; set; }
}

public sealed class UnregistrationParams
{
    [JsonPropertyName("unregisterations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Unregistration> Unregisterations { get; set; }
}

/// <summary>
/// General parameters to unregister a request or notification.
/// </summary>
public sealed class Unregistration
{
    /// <summary>
    /// The id used to unregister the request or notification. Usually an id
    /// provided during the register request.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Id { get; set; }

    /// <summary>
    /// The method to unregister for.
    /// </summary>
    [JsonPropertyName("method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Method { get; set; }
}

public sealed class InitializeParams : I_InitializeParams, IWorkspaceFoldersInitializeParams
{
    /// <summary>
    /// The process Id of the parent process that started
    /// the server.
    /// 
    /// Is `null` if the process has not been started by another process.
    /// If the parent process is not alive then the server should exit.
    /// </summary>
    [JsonPropertyName("processId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32? ProcessId { get; set; }

    /// <summary>
    /// Information about the client
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("clientInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public _InitializeParams.ClientInfoParams? ClientInfo { get; set; }

    /// <summary>
    /// The locale the client is currently showing the user interface
    /// in. This must not necessarily be the locale of the operating
    /// system.
    /// 
    /// Uses IETF language tags as the value's syntax
    /// (See https://en.wikipedia.org/wiki/IETF_language_tag)
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("locale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Locale { get; set; }

    /// <summary>
    /// The rootPath of the workspace. Is null
    /// if no folder is open.
    /// 
    /// @deprecated in favour of rootUri.
    /// </summary>
    [Obsolete("in favour of rootUri.")]
    [JsonPropertyName("rootPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? RootPath { get; set; }

    /// <summary>
    /// The rootUri of the workspace. Is null if no
    /// folder is open. If both `rootPath` and `rootUri` are set
    /// `rootUri` wins.
    /// 
    /// @deprecated in favour of workspaceFolders.
    /// </summary>
    [Obsolete("in favour of workspaceFolders.")]
    [JsonPropertyName("rootUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri? RootUri { get; set; }

    /// <summary>
    /// The capabilities provided by the client (editor or tool)
    /// </summary>
    [JsonPropertyName("capabilities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ClientCapabilities Capabilities { get; set; }

    /// <summary>
    /// User provided initialization options.
    /// </summary>
    [JsonPropertyName("initializationOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? InitializationOptions { get; set; }

    /// <summary>
    /// The initial trace setting. If omitted trace is disabled ('off').
    /// </summary>
    [JsonPropertyName("trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TraceValues? Trace { get; set; }

    /// <summary>
    /// The workspace folders configured in the client when the server starts.
    /// 
    /// This property is only available if the client supports workspace folders.
    /// It can be `null` if the client supports workspace folders but none are
    /// configured.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("workspaceFolders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<WorkspaceFolder>? WorkspaceFolders { get; set; }
}

/// <summary>
/// The initialize parameters
/// </summary>
public sealed class _InitializeParams : I_InitializeParams, IWorkDoneProgressParams
{
    public sealed class ClientInfoParams
    {
        /// <summary>
        /// The name of the client as defined by the client.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.String Name { get; set; }

        /// <summary>
        /// The client's version as defined by the client.
        /// </summary>
        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.String? Version { get; set; }
    }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The process Id of the parent process that started
    /// the server.
    /// 
    /// Is `null` if the process has not been started by another process.
    /// If the parent process is not alive then the server should exit.
    /// </summary>
    [JsonPropertyName("processId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Int32? ProcessId { get; set; }

    /// <summary>
    /// Information about the client
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("clientInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public _InitializeParams.ClientInfoParams? ClientInfo { get; set; }

    /// <summary>
    /// The locale the client is currently showing the user interface
    /// in. This must not necessarily be the locale of the operating
    /// system.
    /// 
    /// Uses IETF language tags as the value's syntax
    /// (See https://en.wikipedia.org/wiki/IETF_language_tag)
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("locale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Locale { get; set; }

    /// <summary>
    /// The rootPath of the workspace. Is null
    /// if no folder is open.
    /// 
    /// @deprecated in favour of rootUri.
    /// </summary>
    [Obsolete("in favour of rootUri.")]
    [JsonPropertyName("rootPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? RootPath { get; set; }

    /// <summary>
    /// The rootUri of the workspace. Is null if no
    /// folder is open. If both `rootPath` and `rootUri` are set
    /// `rootUri` wins.
    /// 
    /// @deprecated in favour of workspaceFolders.
    /// </summary>
    [Obsolete("in favour of workspaceFolders.")]
    [JsonPropertyName("rootUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri? RootUri { get; set; }

    /// <summary>
    /// The capabilities provided by the client (editor or tool)
    /// </summary>
    [JsonPropertyName("capabilities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ClientCapabilities Capabilities { get; set; }

    /// <summary>
    /// User provided initialization options.
    /// </summary>
    [JsonPropertyName("initializationOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? InitializationOptions { get; set; }

    /// <summary>
    /// The initial trace setting. If omitted trace is disabled ('off').
    /// </summary>
    [JsonPropertyName("trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TraceValues? Trace { get; set; }
}

/// <summary>
/// The initialize parameters
/// </summary>
public interface I_InitializeParams
{
    /// <summary>
    /// The process Id of the parent process that started
    /// the server.
    /// 
    /// Is `null` if the process has not been started by another process.
    /// If the parent process is not alive then the server should exit.
    /// </summary>
    [JsonPropertyName("processId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.Int32? ProcessId { get; }

    /// <summary>
    /// Information about the client
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("clientInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public _InitializeParams.ClientInfoParams? ClientInfo { get; }

    /// <summary>
    /// The locale the client is currently showing the user interface
    /// in. This must not necessarily be the locale of the operating
    /// system.
    /// 
    /// Uses IETF language tags as the value's syntax
    /// (See https://en.wikipedia.org/wiki/IETF_language_tag)
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("locale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Locale { get; }

    /// <summary>
    /// The rootPath of the workspace. Is null
    /// if no folder is open.
    /// 
    /// @deprecated in favour of rootUri.
    /// </summary>
    [Obsolete("in favour of rootUri.")]
    [JsonPropertyName("rootPath")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? RootPath { get; }

    /// <summary>
    /// The rootUri of the workspace. Is null if no
    /// folder is open. If both `rootPath` and `rootUri` are set
    /// `rootUri` wins.
    /// 
    /// @deprecated in favour of workspaceFolders.
    /// </summary>
    [Obsolete("in favour of workspaceFolders.")]
    [JsonPropertyName("rootUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public Draco.Lsp.Model.DocumentUri? RootUri { get; }

    /// <summary>
    /// The capabilities provided by the client (editor or tool)
    /// </summary>
    [JsonPropertyName("capabilities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public ClientCapabilities Capabilities { get; }

    /// <summary>
    /// User provided initialization options.
    /// </summary>
    [JsonPropertyName("initializationOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? InitializationOptions { get; }

    /// <summary>
    /// The initial trace setting. If omitted trace is disabled ('off').
    /// </summary>
    [JsonPropertyName("trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TraceValues? Trace { get; }
}

/// <summary>
/// Defines the capabilities provided by the client.
/// </summary>
public sealed class ClientCapabilities
{
    /// <summary>
    /// Workspace specific client capabilities.
    /// </summary>
    [JsonPropertyName("workspace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceClientCapabilities? Workspace { get; set; }

    /// <summary>
    /// Text document specific client capabilities.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TextDocumentClientCapabilities? TextDocument { get; set; }

    /// <summary>
    /// Capabilities specific to the notebook document support.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("notebookDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public NotebookDocumentClientCapabilities? NotebookDocument { get; set; }

    /// <summary>
    /// Window specific client capabilities.
    /// </summary>
    [JsonPropertyName("window")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WindowClientCapabilities? Window { get; set; }

    /// <summary>
    /// General client capabilities.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("general")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GeneralClientCapabilities? General { get; set; }

    /// <summary>
    /// Experimental client capabilities.
    /// </summary>
    [JsonPropertyName("experimental")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Experimental { get; set; }
}

/// <summary>
/// Workspace specific client capabilities.
/// </summary>
public sealed class WorkspaceClientCapabilities
{
    /// <summary>
    /// The client supports applying batch edits
    /// to the workspace by supporting the request
    /// 'workspace/applyEdit'
    /// </summary>
    [JsonPropertyName("applyEdit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ApplyEdit { get; set; }

    /// <summary>
    /// Capabilities specific to `WorkspaceEdit`s.
    /// </summary>
    [JsonPropertyName("workspaceEdit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceEditClientCapabilities? WorkspaceEdit { get; set; }

    /// <summary>
    /// Capabilities specific to the `workspace/didChangeConfiguration` notification.
    /// </summary>
    [JsonPropertyName("didChangeConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DidChangeConfigurationClientCapabilities? DidChangeConfiguration { get; set; }

    /// <summary>
    /// Capabilities specific to the `workspace/didChangeWatchedFiles` notification.
    /// </summary>
    [JsonPropertyName("didChangeWatchedFiles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DidChangeWatchedFilesClientCapabilities? DidChangeWatchedFiles { get; set; }

    /// <summary>
    /// Capabilities specific to the `workspace/symbol` request.
    /// </summary>
    [JsonPropertyName("symbol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceSymbolClientCapabilities? Symbol { get; set; }

    /// <summary>
    /// Capabilities specific to the `workspace/executeCommand` request.
    /// </summary>
    [JsonPropertyName("executeCommand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ExecuteCommandClientCapabilities? ExecuteCommand { get; set; }

    /// <summary>
    /// The client has support for workspace folders.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("workspaceFolders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkspaceFolders { get; set; }

    /// <summary>
    /// The client supports `workspace/configuration` requests.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("configuration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Configuration { get; set; }

    /// <summary>
    /// Capabilities specific to the semantic token requests scoped to the
    /// workspace.
    /// 
    /// @since 3.16.0.
    /// </summary>
    [JsonPropertyName("semanticTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SemanticTokensWorkspaceClientCapabilities? SemanticTokens { get; set; }

    /// <summary>
    /// Capabilities specific to the code lens requests scoped to the
    /// workspace.
    /// 
    /// @since 3.16.0.
    /// </summary>
    [JsonPropertyName("codeLens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeLensWorkspaceClientCapabilities? CodeLens { get; set; }

    /// <summary>
    /// The client has support for file notifications/requests for user operations on files.
    /// 
    /// Since 3.16.0
    /// </summary>
    [JsonPropertyName("fileOperations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationClientCapabilities? FileOperations { get; set; }

    /// <summary>
    /// Capabilities specific to the inline values requests scoped to the
    /// workspace.
    /// 
    /// @since 3.17.0.
    /// </summary>
    [JsonPropertyName("inlineValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlineValueWorkspaceClientCapabilities? InlineValue { get; set; }

    /// <summary>
    /// Capabilities specific to the inlay hint requests scoped to the
    /// workspace.
    /// 
    /// @since 3.17.0.
    /// </summary>
    [JsonPropertyName("inlayHint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlayHintWorkspaceClientCapabilities? InlayHint { get; set; }

    /// <summary>
    /// Capabilities specific to the diagnostic requests scoped to the
    /// workspace.
    /// 
    /// @since 3.17.0.
    /// </summary>
    [JsonPropertyName("diagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiagnosticWorkspaceClientCapabilities? Diagnostics { get; set; }
}

public sealed class WorkspaceEditClientCapabilities
{
    public sealed class ChangeAnnotationSupportCapabilities
    {
        /// <summary>
        /// Whether the client groups edits with equal labels into tree nodes,
        /// for instance all edits labelled with "Changes in Strings" would
        /// be a tree node.
        /// </summary>
        [JsonPropertyName("groupsOnLabel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? GroupsOnLabel { get; set; }
    }

    /// <summary>
    /// The client supports versioned document changes in `WorkspaceEdit`s
    /// </summary>
    [JsonPropertyName("documentChanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DocumentChanges { get; set; }

    /// <summary>
    /// The resource operations the client supports. Clients should at least
    /// support 'create', 'rename' and 'delete' files and folders.
    /// 
    /// @since 3.13.0
    /// </summary>
    [JsonPropertyName("resourceOperations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ResourceOperationKind>? ResourceOperations { get; set; }

    /// <summary>
    /// The failure handling strategy of a client if applying the workspace edit
    /// fails.
    /// 
    /// @since 3.13.0
    /// </summary>
    [JsonPropertyName("failureHandling")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FailureHandlingKind? FailureHandling { get; set; }

    /// <summary>
    /// Whether the client normalizes line endings to the client specific
    /// setting.
    /// If set to `true` the client will normalize line ending characters
    /// in a workspace edit to the client-specified new line
    /// character.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("normalizesLineEndings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? NormalizesLineEndings { get; set; }

    /// <summary>
    /// Whether the client in general supports change annotations on text edits,
    /// create file, rename file and delete file changes.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("changeAnnotationSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceEditClientCapabilities.ChangeAnnotationSupportCapabilities? ChangeAnnotationSupport { get; set; }
}

[JsonConverter(typeof(EnumValueConverter))]
public enum ResourceOperationKind
{
    /// <summary>
    /// Supports creating new files and folders.
    /// </summary>
    [EnumMember(Value = "create")]
    Create,
    /// <summary>
    /// Supports renaming existing files and folders.
    /// </summary>
    [EnumMember(Value = "rename")]
    Rename,
    /// <summary>
    /// Supports deleting existing files and folders.
    /// </summary>
    [EnumMember(Value = "delete")]
    Delete,
}

[JsonConverter(typeof(EnumValueConverter))]
public enum FailureHandlingKind
{
    /// <summary>
    /// Applying the workspace change is simply aborted if one of the changes provided
    /// fails. All operations executed before the failing operation stay executed.
    /// </summary>
    [EnumMember(Value = "abort")]
    Abort,
    /// <summary>
    /// All operations are executed transactional. That means they either all
    /// succeed or no changes at all are applied to the workspace.
    /// </summary>
    [EnumMember(Value = "transactional")]
    Transactional,
    /// <summary>
    /// If the workspace edit contains only textual file changes they are executed transactional.
    /// If resource changes (create, rename or delete file) are part of the change the failure
    /// handling strategy is abort.
    /// </summary>
    [EnumMember(Value = "textOnlyTransactional")]
    TextOnlyTransactional,
    /// <summary>
    /// The client tries to undo the operations already executed. But there is no
    /// guarantee that this is succeeding.
    /// </summary>
    [EnumMember(Value = "undo")]
    Undo,
}

public sealed class DidChangeConfigurationClientCapabilities
{
    /// <summary>
    /// Did change configuration notification supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

public sealed class DidChangeWatchedFilesClientCapabilities
{
    /// <summary>
    /// Did change watched files notification supports dynamic registration. Please note
    /// that the current protocol doesn't support static configuration for file changes
    /// from the server side.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Whether the client has support for {@link  RelativePattern relative pattern}
    /// or not.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("relativePatternSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RelativePatternSupport { get; set; }
}

/// <summary>
/// Client capabilities for a {@link WorkspaceSymbolRequest}.
/// </summary>
public sealed class WorkspaceSymbolClientCapabilities
{
    public sealed class SymbolKindCapabilities
    {
        /// <summary>
        /// The symbol kind values the client supports. When this
        /// property exists the client also guarantees that it will
        /// handle values outside its set gracefully and falls back
        /// to a default value when unknown.
        /// 
        /// If this property is not present the client only supports
        /// the symbol kinds from `File` to `Array` as defined in
        /// the initial version of the protocol.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<SymbolKind>? ValueSet { get; set; }
    }

    public sealed class TagSupportCapabilities
    {
        /// <summary>
        /// The tags supported by the client.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<SymbolTag> ValueSet { get; set; }
    }

    public sealed class ResolveSupportCapabilities
    {
        /// <summary>
        /// The properties that a client can resolve lazily. Usually
        /// `location.range`
        /// </summary>
        [JsonPropertyName("properties")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<System.String> Properties { get; set; }
    }

    /// <summary>
    /// Symbol request supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Specific capabilities for the `SymbolKind` in the `workspace/symbol` request.
    /// </summary>
    [JsonPropertyName("symbolKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceSymbolClientCapabilities.SymbolKindCapabilities? SymbolKind { get; set; }

    /// <summary>
    /// The client supports tags on `SymbolInformation`.
    /// Clients supporting tags have to handle unknown tags gracefully.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tagSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceSymbolClientCapabilities.TagSupportCapabilities? TagSupport { get; set; }

    /// <summary>
    /// The client support partial workspace symbols. The client will send the
    /// request `workspaceSymbol/resolve` to the server to resolve additional
    /// properties.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("resolveSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceSymbolClientCapabilities.ResolveSupportCapabilities? ResolveSupport { get; set; }
}

/// <summary>
/// The client capabilities of a {@link ExecuteCommandRequest}.
/// </summary>
public sealed class ExecuteCommandClientCapabilities
{
    /// <summary>
    /// Execute command supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensWorkspaceClientCapabilities
{
    /// <summary>
    /// Whether the client implementation supports a refresh request sent from
    /// the server to the client.
    /// 
    /// Note that this event is global and will force the client to refresh all
    /// semantic tokens currently shown. It should be used with absolute care
    /// and is useful for situation where a server for example detects a project
    /// wide change that requires such a calculation.
    /// </summary>
    [JsonPropertyName("refreshSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RefreshSupport { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class CodeLensWorkspaceClientCapabilities
{
    /// <summary>
    /// Whether the client implementation supports a refresh request sent from the
    /// server to the client.
    /// 
    /// Note that this event is global and will force the client to refresh all
    /// code lenses currently shown. It should be used with absolute care and is
    /// useful for situation where a server for example detect a project wide
    /// change that requires such a calculation.
    /// </summary>
    [JsonPropertyName("refreshSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RefreshSupport { get; set; }
}

/// <summary>
/// Capabilities relating to events from file operations by the user in the client.
/// 
/// These events do not come from the file system, they come from user operations
/// like renaming a file in the UI.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationClientCapabilities
{
    /// <summary>
    /// Whether the client supports dynamic registration for file requests/notifications.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client has support for sending didCreateFiles notifications.
    /// </summary>
    [JsonPropertyName("didCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DidCreate { get; set; }

    /// <summary>
    /// The client has support for sending willCreateFiles requests.
    /// </summary>
    [JsonPropertyName("willCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillCreate { get; set; }

    /// <summary>
    /// The client has support for sending didRenameFiles notifications.
    /// </summary>
    [JsonPropertyName("didRename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DidRename { get; set; }

    /// <summary>
    /// The client has support for sending willRenameFiles requests.
    /// </summary>
    [JsonPropertyName("willRename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillRename { get; set; }

    /// <summary>
    /// The client has support for sending didDeleteFiles notifications.
    /// </summary>
    [JsonPropertyName("didDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DidDelete { get; set; }

    /// <summary>
    /// The client has support for sending willDeleteFiles requests.
    /// </summary>
    [JsonPropertyName("willDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillDelete { get; set; }
}

/// <summary>
/// Client workspace capabilities specific to inline values.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueWorkspaceClientCapabilities
{
    /// <summary>
    /// Whether the client implementation supports a refresh request sent from the
    /// server to the client.
    /// 
    /// Note that this event is global and will force the client to refresh all
    /// inline values currently shown. It should be used with absolute care and is
    /// useful for situation where a server for example detects a project wide
    /// change that requires such a calculation.
    /// </summary>
    [JsonPropertyName("refreshSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RefreshSupport { get; set; }
}

/// <summary>
/// Client workspace capabilities specific to inlay hints.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintWorkspaceClientCapabilities
{
    /// <summary>
    /// Whether the client implementation supports a refresh request sent from
    /// the server to the client.
    /// 
    /// Note that this event is global and will force the client to refresh all
    /// inlay hints currently shown. It should be used with absolute care and
    /// is useful for situation where a server for example detects a project wide
    /// change that requires such a calculation.
    /// </summary>
    [JsonPropertyName("refreshSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RefreshSupport { get; set; }
}

/// <summary>
/// Workspace client capabilities specific to diagnostic pull requests.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DiagnosticWorkspaceClientCapabilities
{
    /// <summary>
    /// Whether the client implementation supports a refresh request sent from
    /// the server to the client.
    /// 
    /// Note that this event is global and will force the client to refresh all
    /// pulled diagnostics currently shown. It should be used with absolute care and
    /// is useful for situation where a server for example detects a project wide
    /// change that requires such a calculation.
    /// </summary>
    [JsonPropertyName("refreshSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RefreshSupport { get; set; }
}

/// <summary>
/// Text document specific client capabilities.
/// </summary>
public sealed class TextDocumentClientCapabilities
{
    /// <summary>
    /// Defines which synchronization capabilities the client supports.
    /// </summary>
    [JsonPropertyName("synchronization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TextDocumentSyncClientCapabilities? Synchronization { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/completion` request.
    /// </summary>
    [JsonPropertyName("completion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionClientCapabilities? Completion { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/hover` request.
    /// </summary>
    [JsonPropertyName("hover")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public HoverClientCapabilities? Hover { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/signatureHelp` request.
    /// </summary>
    [JsonPropertyName("signatureHelp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SignatureHelpClientCapabilities? SignatureHelp { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/declaration` request.
    /// 
    /// @since 3.14.0
    /// </summary>
    [JsonPropertyName("declaration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DeclarationClientCapabilities? Declaration { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/definition` request.
    /// </summary>
    [JsonPropertyName("definition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DefinitionClientCapabilities? Definition { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/typeDefinition` request.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("typeDefinition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TypeDefinitionClientCapabilities? TypeDefinition { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/implementation` request.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("implementation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImplementationClientCapabilities? Implementation { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/references` request.
    /// </summary>
    [JsonPropertyName("references")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ReferenceClientCapabilities? References { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/documentHighlight` request.
    /// </summary>
    [JsonPropertyName("documentHighlight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentHighlightClientCapabilities? DocumentHighlight { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/documentSymbol` request.
    /// </summary>
    [JsonPropertyName("documentSymbol")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentSymbolClientCapabilities? DocumentSymbol { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/codeAction` request.
    /// </summary>
    [JsonPropertyName("codeAction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeActionClientCapabilities? CodeAction { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/codeLens` request.
    /// </summary>
    [JsonPropertyName("codeLens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeLensClientCapabilities? CodeLens { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/documentLink` request.
    /// </summary>
    [JsonPropertyName("documentLink")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentLinkClientCapabilities? DocumentLink { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/documentColor` and the
    /// `textDocument/colorPresentation` request.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("colorProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentColorClientCapabilities? ColorProvider { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/formatting` request.
    /// </summary>
    [JsonPropertyName("formatting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentFormattingClientCapabilities? Formatting { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/rangeFormatting` request.
    /// </summary>
    [JsonPropertyName("rangeFormatting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentRangeFormattingClientCapabilities? RangeFormatting { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/onTypeFormatting` request.
    /// </summary>
    [JsonPropertyName("onTypeFormatting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentOnTypeFormattingClientCapabilities? OnTypeFormatting { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/rename` request.
    /// </summary>
    [JsonPropertyName("rename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RenameClientCapabilities? Rename { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/foldingRange` request.
    /// 
    /// @since 3.10.0
    /// </summary>
    [JsonPropertyName("foldingRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FoldingRangeClientCapabilities? FoldingRange { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/selectionRange` request.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("selectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SelectionRangeClientCapabilities? SelectionRange { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/publishDiagnostics` notification.
    /// </summary>
    [JsonPropertyName("publishDiagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PublishDiagnosticsClientCapabilities? PublishDiagnostics { get; set; }

    /// <summary>
    /// Capabilities specific to the various call hierarchy requests.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("callHierarchy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CallHierarchyClientCapabilities? CallHierarchy { get; set; }

    /// <summary>
    /// Capabilities specific to the various semantic token request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("semanticTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SemanticTokensClientCapabilities? SemanticTokens { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/linkedEditingRange` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("linkedEditingRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LinkedEditingRangeClientCapabilities? LinkedEditingRange { get; set; }

    /// <summary>
    /// Client capabilities specific to the `textDocument/moniker` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("moniker")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MonikerClientCapabilities? Moniker { get; set; }

    /// <summary>
    /// Capabilities specific to the various type hierarchy requests.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("typeHierarchy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TypeHierarchyClientCapabilities? TypeHierarchy { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/inlineValue` request.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("inlineValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlineValueClientCapabilities? InlineValue { get; set; }

    /// <summary>
    /// Capabilities specific to the `textDocument/inlayHint` request.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("inlayHint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlayHintClientCapabilities? InlayHint { get; set; }

    /// <summary>
    /// Capabilities specific to the diagnostic pull model.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("diagnostic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DiagnosticClientCapabilities? Diagnostic { get; set; }
}

public sealed class TextDocumentSyncClientCapabilities
{
    /// <summary>
    /// Whether text document synchronization supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports sending will save notifications.
    /// </summary>
    [JsonPropertyName("willSave")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillSave { get; set; }

    /// <summary>
    /// The client supports sending a will save request and
    /// waits for a response providing text edits which will
    /// be applied to the document before it is saved.
    /// </summary>
    [JsonPropertyName("willSaveWaitUntil")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillSaveWaitUntil { get; set; }

    /// <summary>
    /// The client supports did save notifications.
    /// </summary>
    [JsonPropertyName("didSave")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DidSave { get; set; }
}

/// <summary>
/// Completion client capabilities
/// </summary>
public sealed class CompletionClientCapabilities
{
    public sealed class CompletionItemCapabilities
    {
        public sealed class TagSupportCapabilities
        {
            /// <summary>
            /// The tags supported by the client.
            /// </summary>
            [JsonPropertyName("valueSet")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required IList<CompletionItemTag> ValueSet { get; set; }
        }

        public sealed class ResolveSupportCapabilities
        {
            /// <summary>
            /// The properties that a client can resolve lazily.
            /// </summary>
            [JsonPropertyName("properties")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required IList<System.String> Properties { get; set; }
        }

        public sealed class InsertTextModeSupportCapabilities
        {
            [JsonPropertyName("valueSet")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required IList<InsertTextMode> ValueSet { get; set; }
        }

        /// <summary>
        /// Client supports snippets as insert text.
        /// 
        /// A snippet can define tab stops and placeholders with `$1`, `$2`
        /// and `${3:foo}`. `$0` defines the final tab stop, it defaults to
        /// the end of the snippet. Placeholders with equal identifiers are linked,
        /// that is typing in one will update others too.
        /// </summary>
        [JsonPropertyName("snippetSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? SnippetSupport { get; set; }

        /// <summary>
        /// Client supports commit characters on a completion item.
        /// </summary>
        [JsonPropertyName("commitCharactersSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? CommitCharactersSupport { get; set; }

        /// <summary>
        /// Client supports the following content formats for the documentation
        /// property. The order describes the preferred format of the client.
        /// </summary>
        [JsonPropertyName("documentationFormat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<MarkupKind>? DocumentationFormat { get; set; }

        /// <summary>
        /// Client supports the deprecated property on a completion item.
        /// </summary>
        [JsonPropertyName("deprecatedSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? DeprecatedSupport { get; set; }

        /// <summary>
        /// Client supports the preselect property on a completion item.
        /// </summary>
        [JsonPropertyName("preselectSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? PreselectSupport { get; set; }

        /// <summary>
        /// Client supports the tag property on a completion item. Clients supporting
        /// tags have to handle unknown tags gracefully. Clients especially need to
        /// preserve unknown tags when sending a completion item back to the server in
        /// a resolve call.
        /// 
        /// @since 3.15.0
        /// </summary>
        [JsonPropertyName("tagSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CompletionClientCapabilities.CompletionItemCapabilities.TagSupportCapabilities? TagSupport { get; set; }

        /// <summary>
        /// Client support insert replace edit to control different behavior if a
        /// completion item is inserted in the text or should replace text.
        /// 
        /// @since 3.16.0
        /// </summary>
        [JsonPropertyName("insertReplaceSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? InsertReplaceSupport { get; set; }

        /// <summary>
        /// Indicates which properties a client can resolve lazily on a completion
        /// item. Before version 3.16.0 only the predefined properties `documentation`
        /// and `details` could be resolved lazily.
        /// 
        /// @since 3.16.0
        /// </summary>
        [JsonPropertyName("resolveSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CompletionClientCapabilities.CompletionItemCapabilities.ResolveSupportCapabilities? ResolveSupport { get; set; }

        /// <summary>
        /// The client supports the `insertTextMode` property on
        /// a completion item to override the whitespace handling mode
        /// as defined by the client (see `insertTextMode`).
        /// 
        /// @since 3.16.0
        /// </summary>
        [JsonPropertyName("insertTextModeSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CompletionClientCapabilities.CompletionItemCapabilities.InsertTextModeSupportCapabilities? InsertTextModeSupport { get; set; }

        /// <summary>
        /// The client has support for completion item label
        /// details (see also `CompletionItemLabelDetails`).
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("labelDetailsSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? LabelDetailsSupport { get; set; }
    }

    public sealed class CompletionItemKindCapabilities
    {
        /// <summary>
        /// The completion item kind values the client supports. When this
        /// property exists the client also guarantees that it will
        /// handle values outside its set gracefully and falls back
        /// to a default value when unknown.
        /// 
        /// If this property is not present the client only supports
        /// the completion items kinds from `Text` to `Reference` as defined in
        /// the initial version of the protocol.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<CompletionItemKind>? ValueSet { get; set; }
    }

    public sealed class CompletionListCapabilities
    {
        /// <summary>
        /// The client supports the following itemDefaults on
        /// a completion list.
        /// 
        /// The value lists the supported property names of the
        /// `CompletionList.itemDefaults` object. If omitted
        /// no properties are supported.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("itemDefaults")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<System.String>? ItemDefaults { get; set; }
    }

    /// <summary>
    /// Whether completion supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports the following `CompletionItem` specific
    /// capabilities.
    /// </summary>
    [JsonPropertyName("completionItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionClientCapabilities.CompletionItemCapabilities? CompletionItem { get; set; }

    [JsonPropertyName("completionItemKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionClientCapabilities.CompletionItemKindCapabilities? CompletionItemKind { get; set; }

    /// <summary>
    /// Defines how the client handles whitespace and indentation
    /// when accepting a completion item that uses multi line
    /// text in either `insertText` or `textEdit`.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("insertTextMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InsertTextMode? InsertTextMode { get; set; }

    /// <summary>
    /// The client supports to send additional context information for a
    /// `textDocument/completion` request.
    /// </summary>
    [JsonPropertyName("contextSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ContextSupport { get; set; }

    /// <summary>
    /// The client supports the following `CompletionList` specific
    /// capabilities.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("completionList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionClientCapabilities.CompletionListCapabilities? CompletionList { get; set; }
}

/// <summary>
/// Completion item tags are extra annotations that tweak the rendering of a completion
/// item.
/// 
/// @since 3.15.0
/// </summary>
public enum CompletionItemTag
{
    /// <summary>
    /// Render a completion as obsolete, usually using a strike-out.
    /// </summary>
    Deprecated = 1,
}

/// <summary>
/// How whitespace and indentation is handled during completion
/// item insertion.
/// 
/// @since 3.16.0
/// </summary>
public enum InsertTextMode
{
    /// <summary>
    /// The insertion or replace strings is taken as it is. If the
    /// value is multi line the lines below the cursor will be
    /// inserted using the indentation defined in the string value.
    /// The client will not apply any kind of adjustments to the
    /// string.
    /// </summary>
    AsIs = 1,
    /// <summary>
    /// The editor adjusts leading whitespace of new lines so that
    /// they match the indentation up to the cursor of the line for
    /// which the item is accepted.
    /// 
    /// Consider a line like this: < 2 tabs><cursor>< 3 tabs>foo. Accepting a
    /// multi line completion item is indented using 2 tabs and all
    /// following lines inserted will be indented using 2 tabs as well.
    /// </summary>
    AdjustIndentation = 2,
}

/// <summary>
/// The kind of a completion entry.
/// </summary>
public enum CompletionItemKind
{
    Text = 1,
    Method = 2,
    Function = 3,
    Constructor = 4,
    Field = 5,
    Variable = 6,
    Class = 7,
    Interface = 8,
    Module = 9,
    Property = 10,
    Unit = 11,
    Value = 12,
    Enum = 13,
    Keyword = 14,
    Snippet = 15,
    Color = 16,
    File = 17,
    Reference = 18,
    Folder = 19,
    EnumMember = 20,
    Constant = 21,
    Struct = 22,
    Event = 23,
    Operator = 24,
    TypeParameter = 25,
}

public sealed class HoverClientCapabilities
{
    /// <summary>
    /// Whether hover supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Client supports the following content formats for the content
    /// property. The order describes the preferred format of the client.
    /// </summary>
    [JsonPropertyName("contentFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<MarkupKind>? ContentFormat { get; set; }
}

/// <summary>
/// Client Capabilities for a {@link SignatureHelpRequest}.
/// </summary>
public sealed class SignatureHelpClientCapabilities
{
    public sealed class SignatureInformationCapabilities
    {
        public sealed class ParameterInformationCapabilities
        {
            /// <summary>
            /// The client supports processing label offsets instead of a
            /// simple label string.
            /// 
            /// @since 3.14.0
            /// </summary>
            [JsonPropertyName("labelOffsetSupport")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public System.Boolean? LabelOffsetSupport { get; set; }
        }

        /// <summary>
        /// Client supports the following content formats for the documentation
        /// property. The order describes the preferred format of the client.
        /// </summary>
        [JsonPropertyName("documentationFormat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<MarkupKind>? DocumentationFormat { get; set; }

        /// <summary>
        /// Client capabilities specific to parameter information.
        /// </summary>
        [JsonPropertyName("parameterInformation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SignatureHelpClientCapabilities.SignatureInformationCapabilities.ParameterInformationCapabilities? ParameterInformation { get; set; }

        /// <summary>
        /// The client supports the `activeParameter` property on `SignatureInformation`
        /// literal.
        /// 
        /// @since 3.16.0
        /// </summary>
        [JsonPropertyName("activeParameterSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? ActiveParameterSupport { get; set; }
    }

    /// <summary>
    /// Whether signature help supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports the following `SignatureInformation`
    /// specific properties.
    /// </summary>
    [JsonPropertyName("signatureInformation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SignatureHelpClientCapabilities.SignatureInformationCapabilities? SignatureInformation { get; set; }

    /// <summary>
    /// The client supports to send additional context information for a
    /// `textDocument/signatureHelp` request. A client that opts into
    /// contextSupport will also support the `retriggerCharacters` on
    /// `SignatureHelpOptions`.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("contextSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ContextSupport { get; set; }
}

/// <summary>
/// @since 3.14.0
/// </summary>
public sealed class DeclarationClientCapabilities
{
    /// <summary>
    /// Whether declaration supports dynamic registration. If this is set to `true`
    /// the client supports the new `DeclarationRegistrationOptions` return value
    /// for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports additional metadata in the form of declaration links.
    /// </summary>
    [JsonPropertyName("linkSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LinkSupport { get; set; }
}

/// <summary>
/// Client Capabilities for a {@link DefinitionRequest}.
/// </summary>
public sealed class DefinitionClientCapabilities
{
    /// <summary>
    /// Whether definition supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports additional metadata in the form of definition links.
    /// 
    /// @since 3.14.0
    /// </summary>
    [JsonPropertyName("linkSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LinkSupport { get; set; }
}

/// <summary>
/// Since 3.6.0
/// </summary>
public sealed class TypeDefinitionClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `TypeDefinitionRegistrationOptions` return value
    /// for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports additional metadata in the form of definition links.
    /// 
    /// Since 3.14.0
    /// </summary>
    [JsonPropertyName("linkSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LinkSupport { get; set; }
}

/// <summary>
/// @since 3.6.0
/// </summary>
public sealed class ImplementationClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `ImplementationRegistrationOptions` return value
    /// for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports additional metadata in the form of definition links.
    /// 
    /// @since 3.14.0
    /// </summary>
    [JsonPropertyName("linkSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LinkSupport { get; set; }
}

/// <summary>
/// Client Capabilities for a {@link ReferencesRequest}.
/// </summary>
public sealed class ReferenceClientCapabilities
{
    /// <summary>
    /// Whether references supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client Capabilities for a {@link DocumentHighlightRequest}.
/// </summary>
public sealed class DocumentHighlightClientCapabilities
{
    /// <summary>
    /// Whether document highlight supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client Capabilities for a {@link DocumentSymbolRequest}.
/// </summary>
public sealed class DocumentSymbolClientCapabilities
{
    public sealed class SymbolKindCapabilities
    {
        /// <summary>
        /// The symbol kind values the client supports. When this
        /// property exists the client also guarantees that it will
        /// handle values outside its set gracefully and falls back
        /// to a default value when unknown.
        /// 
        /// If this property is not present the client only supports
        /// the symbol kinds from `File` to `Array` as defined in
        /// the initial version of the protocol.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<SymbolKind>? ValueSet { get; set; }
    }

    public sealed class TagSupportCapabilities
    {
        /// <summary>
        /// The tags supported by the client.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<SymbolTag> ValueSet { get; set; }
    }

    /// <summary>
    /// Whether document symbol supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Specific capabilities for the `SymbolKind` in the
    /// `textDocument/documentSymbol` request.
    /// </summary>
    [JsonPropertyName("symbolKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentSymbolClientCapabilities.SymbolKindCapabilities? SymbolKind { get; set; }

    /// <summary>
    /// The client supports hierarchical document symbols.
    /// </summary>
    [JsonPropertyName("hierarchicalDocumentSymbolSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? HierarchicalDocumentSymbolSupport { get; set; }

    /// <summary>
    /// The client supports tags on `SymbolInformation`. Tags are supported on
    /// `DocumentSymbol` if `hierarchicalDocumentSymbolSupport` is set to true.
    /// Clients supporting tags have to handle unknown tags gracefully.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tagSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentSymbolClientCapabilities.TagSupportCapabilities? TagSupport { get; set; }

    /// <summary>
    /// The client supports an additional label presented in the UI when
    /// registering a document symbol provider.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("labelSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LabelSupport { get; set; }
}

/// <summary>
/// The Client Capabilities of a {@link CodeActionRequest}.
/// </summary>
public sealed class CodeActionClientCapabilities
{
    public sealed class CodeActionLiteralSupportCapabilities
    {
        public sealed class CodeActionKindCapabilities
        {
            /// <summary>
            /// The code action kind values the client supports. When this
            /// property exists the client also guarantees that it will
            /// handle values outside its set gracefully and falls back
            /// to a default value when unknown.
            /// </summary>
            [JsonPropertyName("valueSet")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required IList<CodeActionKind> ValueSet { get; set; }
        }

        /// <summary>
        /// The code action kind is support with the following value
        /// set.
        /// </summary>
        [JsonPropertyName("codeActionKind")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required CodeActionClientCapabilities.CodeActionLiteralSupportCapabilities.CodeActionKindCapabilities CodeActionKind { get; set; }
    }

    public sealed class ResolveSupportCapabilities
    {
        /// <summary>
        /// The properties that a client can resolve lazily.
        /// </summary>
        [JsonPropertyName("properties")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<System.String> Properties { get; set; }
    }

    /// <summary>
    /// Whether code action supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client support code action literals of type `CodeAction` as a valid
    /// response of the `textDocument/codeAction` request. If the property is not
    /// set the request can only return `Command` literals.
    /// 
    /// @since 3.8.0
    /// </summary>
    [JsonPropertyName("codeActionLiteralSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeActionClientCapabilities.CodeActionLiteralSupportCapabilities? CodeActionLiteralSupport { get; set; }

    /// <summary>
    /// Whether code action supports the `isPreferred` property.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("isPreferredSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IsPreferredSupport { get; set; }

    /// <summary>
    /// Whether code action supports the `disabled` property.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("disabledSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DisabledSupport { get; set; }

    /// <summary>
    /// Whether code action supports the `data` property which is
    /// preserved between a `textDocument/codeAction` and a
    /// `codeAction/resolve` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("dataSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DataSupport { get; set; }

    /// <summary>
    /// Whether the client supports resolving additional code action
    /// properties via a separate `codeAction/resolve` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("resolveSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeActionClientCapabilities.ResolveSupportCapabilities? ResolveSupport { get; set; }

    /// <summary>
    /// Whether the client honors the change annotations in
    /// text edits and resource operations returned via the
    /// `CodeAction#edit` property by for example presenting
    /// the workspace edit in the user interface and asking
    /// for confirmation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("honorsChangeAnnotations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? HonorsChangeAnnotations { get; set; }
}

/// <summary>
/// A set of predefined code action kinds
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum CodeActionKind
{
    /// <summary>
    /// Empty kind.
    /// </summary>
    [EnumMember(Value = "")]
    Empty,
    /// <summary>
    /// Base kind for quickfix actions: 'quickfix'
    /// </summary>
    [EnumMember(Value = "quickfix")]
    QuickFix,
    /// <summary>
    /// Base kind for refactoring actions: 'refactor'
    /// </summary>
    [EnumMember(Value = "refactor")]
    Refactor,
    /// <summary>
    /// Base kind for refactoring extraction actions: 'refactor.extract'
    /// 
    /// Example extract actions:
    /// 
    /// - Extract method
    /// - Extract function
    /// - Extract variable
    /// - Extract interface from class
    /// - ...
    /// </summary>
    [EnumMember(Value = "refactor.extract")]
    RefactorExtract,
    /// <summary>
    /// Base kind for refactoring inline actions: 'refactor.inline'
    /// 
    /// Example inline actions:
    /// 
    /// - Inline function
    /// - Inline variable
    /// - Inline constant
    /// - ...
    /// </summary>
    [EnumMember(Value = "refactor.inline")]
    RefactorInline,
    /// <summary>
    /// Base kind for refactoring rewrite actions: 'refactor.rewrite'
    /// 
    /// Example rewrite actions:
    /// 
    /// - Convert JavaScript function to class
    /// - Add or remove parameter
    /// - Encapsulate field
    /// - Make method static
    /// - Move method to base class
    /// - ...
    /// </summary>
    [EnumMember(Value = "refactor.rewrite")]
    RefactorRewrite,
    /// <summary>
    /// Base kind for source actions: `source`
    /// 
    /// Source code actions apply to the entire file.
    /// </summary>
    [EnumMember(Value = "source")]
    Source,
    /// <summary>
    /// Base kind for an organize imports source action: `source.organizeImports`
    /// </summary>
    [EnumMember(Value = "source.organizeImports")]
    SourceOrganizeImports,
    /// <summary>
    /// Base kind for auto-fix source actions: `source.fixAll`.
    /// 
    /// Fix all actions automatically fix errors that have a clear fix that do not require user input.
    /// They should not suppress errors or perform unsafe fixes such as generating new types or classes.
    /// 
    /// @since 3.15.0
    /// </summary>
    [EnumMember(Value = "source.fixAll")]
    SourceFixAll,
}

/// <summary>
/// The client capabilities  of a {@link CodeLensRequest}.
/// </summary>
public sealed class CodeLensClientCapabilities
{
    /// <summary>
    /// Whether code lens supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// The client capabilities of a {@link DocumentLinkRequest}.
/// </summary>
public sealed class DocumentLinkClientCapabilities
{
    /// <summary>
    /// Whether document link supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Whether the client supports the `tooltip` property on `DocumentLink`.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("tooltipSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? TooltipSupport { get; set; }
}

public sealed class DocumentColorClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `DocumentColorRegistrationOptions` return value
    /// for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client capabilities of a {@link DocumentFormattingRequest}.
/// </summary>
public sealed class DocumentFormattingClientCapabilities
{
    /// <summary>
    /// Whether formatting supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client capabilities of a {@link DocumentRangeFormattingRequest}.
/// </summary>
public sealed class DocumentRangeFormattingClientCapabilities
{
    /// <summary>
    /// Whether range formatting supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client capabilities of a {@link DocumentOnTypeFormattingRequest}.
/// </summary>
public sealed class DocumentOnTypeFormattingClientCapabilities
{
    /// <summary>
    /// Whether on type formatting supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

public sealed class RenameClientCapabilities
{
    /// <summary>
    /// Whether rename supports dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Client supports testing for validity of rename operations
    /// before execution.
    /// 
    /// @since 3.12.0
    /// </summary>
    [JsonPropertyName("prepareSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PrepareSupport { get; set; }

    /// <summary>
    /// Client supports the default behavior result.
    /// 
    /// The value indicates the default behavior used by the
    /// client.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("prepareSupportDefaultBehavior")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PrepareSupportDefaultBehavior? PrepareSupportDefaultBehavior { get; set; }

    /// <summary>
    /// Whether the client honors the change annotations in
    /// text edits and resource operations returned via the
    /// rename request's workspace edit by for example presenting
    /// the workspace edit in the user interface and asking
    /// for confirmation.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("honorsChangeAnnotations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? HonorsChangeAnnotations { get; set; }
}

public enum PrepareSupportDefaultBehavior
{
    /// <summary>
    /// The client's default behavior is to select the identifier
    /// according the to language's syntax rule.
    /// </summary>
    Identifier = 1,
}

public sealed class FoldingRangeClientCapabilities
{
    public sealed class FoldingRangeKindCapabilities
    {
        /// <summary>
        /// The folding range kind values the client supports. When this
        /// property exists the client also guarantees that it will
        /// handle values outside its set gracefully and falls back
        /// to a default value when unknown.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<FoldingRangeKind>? ValueSet { get; set; }
    }

    public sealed class FoldingRangeCapabilities
    {
        /// <summary>
        /// If set, the client signals that it supports setting collapsedText on
        /// folding ranges to display custom labels instead of the default text.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("collapsedText")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? CollapsedText { get; set; }
    }

    /// <summary>
    /// Whether implementation supports dynamic registration for folding range
    /// providers. If this is set to `true` the client supports the new
    /// `FoldingRangeRegistrationOptions` return value for the corresponding
    /// server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The maximum number of folding ranges that the client prefers to receive
    /// per document. The value serves as a hint, servers are free to follow the
    /// limit.
    /// </summary>
    [JsonPropertyName("rangeLimit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? RangeLimit { get; set; }

    /// <summary>
    /// If set, the client signals that it only supports folding complete lines.
    /// If set, client will ignore specified `startCharacter` and `endCharacter`
    /// properties in a FoldingRange.
    /// </summary>
    [JsonPropertyName("lineFoldingOnly")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? LineFoldingOnly { get; set; }

    /// <summary>
    /// Specific options for the folding range kind.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("foldingRangeKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FoldingRangeClientCapabilities.FoldingRangeKindCapabilities? FoldingRangeKind { get; set; }

    /// <summary>
    /// Specific options for the folding range.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("foldingRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FoldingRangeClientCapabilities.FoldingRangeCapabilities? FoldingRange { get; set; }
}

public sealed class SelectionRangeClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration for selection range providers. If this is set to `true`
    /// the client supports the new `SelectionRangeRegistrationOptions` return value for the corresponding server
    /// capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// The publish diagnostic client capabilities.
/// </summary>
public sealed class PublishDiagnosticsClientCapabilities
{
    public sealed class TagSupportCapabilities
    {
        /// <summary>
        /// The tags supported by the client.
        /// </summary>
        [JsonPropertyName("valueSet")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<DiagnosticTag> ValueSet { get; set; }
    }

    /// <summary>
    /// Whether the clients accepts diagnostics with related information.
    /// </summary>
    [JsonPropertyName("relatedInformation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RelatedInformation { get; set; }

    /// <summary>
    /// Client supports the tag property to provide meta data about a diagnostic.
    /// Clients supporting tags have to handle unknown tags gracefully.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("tagSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PublishDiagnosticsClientCapabilities.TagSupportCapabilities? TagSupport { get; set; }

    /// <summary>
    /// Whether the client interprets the version property of the
    /// `textDocument/publishDiagnostics` notification's parameter.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("versionSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? VersionSupport { get; set; }

    /// <summary>
    /// Client supports a codeDescription property
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("codeDescriptionSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? CodeDescriptionSupport { get; set; }

    /// <summary>
    /// Whether code action supports the `data` property which is
    /// preserved between a `textDocument/publishDiagnostics` and
    /// `textDocument/codeAction` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("dataSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DataSupport { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class CallHierarchyClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// @since 3.16.0
/// </summary>
public sealed class SemanticTokensClientCapabilities
{
    public sealed class RequestsCapabilities
    {
        public sealed class RangeCapabilities
        {
        }

        public sealed class FullCapabilities
        {
            /// <summary>
            /// The client will send the `textDocument/semanticTokens/full/delta` request if
            /// the server provides a corresponding handler.
            /// </summary>
            [JsonPropertyName("delta")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public System.Boolean? Delta { get; set; }
        }

        /// <summary>
        /// The client will send the `textDocument/semanticTokens/range` request if
        /// the server provides a corresponding handler.
        /// </summary>
        [JsonPropertyName("range")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OneOf<System.Boolean, SemanticTokensClientCapabilities.RequestsCapabilities.RangeCapabilities>? Range { get; set; }

        /// <summary>
        /// The client will send the `textDocument/semanticTokens/full` request if
        /// the server provides a corresponding handler.
        /// </summary>
        [JsonPropertyName("full")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OneOf<System.Boolean, SemanticTokensClientCapabilities.RequestsCapabilities.FullCapabilities>? Full { get; set; }
    }

    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Which requests the client supports and might send to the server
    /// depending on the server's capability. Please note that clients might not
    /// show semantic tokens or degrade some of the user experience if a range
    /// or full request is advertised by the client but not provided by the
    /// server. If for example the client capability `requests.full` and
    /// `request.range` are both set to true but the server only provides a
    /// range provider the client might not render a minimap correctly or might
    /// even decide to not show any semantic tokens at all.
    /// </summary>
    [JsonPropertyName("requests")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SemanticTokensClientCapabilities.RequestsCapabilities Requests { get; set; }

    /// <summary>
    /// The token types that the client supports.
    /// </summary>
    [JsonPropertyName("tokenTypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> TokenTypes { get; set; }

    /// <summary>
    /// The token modifiers that the client supports.
    /// </summary>
    [JsonPropertyName("tokenModifiers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> TokenModifiers { get; set; }

    /// <summary>
    /// The token formats the clients supports.
    /// </summary>
    [JsonPropertyName("formats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<TokenFormat> Formats { get; set; }

    /// <summary>
    /// Whether the client supports tokens that can overlap each other.
    /// </summary>
    [JsonPropertyName("overlappingTokenSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? OverlappingTokenSupport { get; set; }

    /// <summary>
    /// Whether the client supports tokens that can span multiple lines.
    /// </summary>
    [JsonPropertyName("multilineTokenSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? MultilineTokenSupport { get; set; }

    /// <summary>
    /// Whether the client allows the server to actively cancel a
    /// semantic token request, e.g. supports returning
    /// LSPErrorCodes.ServerCancelled. If a server does the client
    /// needs to retrigger the request.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("serverCancelSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ServerCancelSupport { get; set; }

    /// <summary>
    /// Whether the client uses semantic tokens to augment existing
    /// syntax tokens. If set to `true` client side created syntax
    /// tokens and semantic tokens are both used for colorization. If
    /// set to `false` the client only uses the returned semantic tokens
    /// for colorization.
    /// 
    /// If the value is `undefined` then the client behavior is not
    /// specified.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("augmentsSyntaxTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? AugmentsSyntaxTokens { get; set; }
}

[JsonConverter(typeof(EnumValueConverter))]
public enum TokenFormat
{
    [EnumMember(Value = "relative")]
    Relative,
}

/// <summary>
/// Client capabilities for the linked editing range request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class LinkedEditingRangeClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client capabilities specific to the moniker request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class MonikerClientCapabilities
{
    /// <summary>
    /// Whether moniker supports dynamic registration. If this is set to `true`
    /// the client supports the new `MonikerRegistrationOptions` return value
    /// for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// @since 3.17.0
/// </summary>
public sealed class TypeHierarchyClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Client capabilities specific to inline values.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration for inline value providers.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }
}

/// <summary>
/// Inlay hint client capabilities.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlayHintClientCapabilities
{
    public sealed class ResolveSupportCapabilities
    {
        /// <summary>
        /// The properties that a client can resolve lazily.
        /// </summary>
        [JsonPropertyName("properties")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<System.String> Properties { get; set; }
    }

    /// <summary>
    /// Whether inlay hints support dynamic registration.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Indicates which properties a client can resolve lazily on an inlay
    /// hint.
    /// </summary>
    [JsonPropertyName("resolveSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InlayHintClientCapabilities.ResolveSupportCapabilities? ResolveSupport { get; set; }
}

/// <summary>
/// Client capabilities specific to diagnostic pull requests.
/// 
/// @since 3.17.0
/// </summary>
public sealed class DiagnosticClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is set to `true`
    /// the client supports the new `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// Whether the clients supports related documents for document diagnostic pulls.
    /// </summary>
    [JsonPropertyName("relatedDocumentSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? RelatedDocumentSupport { get; set; }
}

/// <summary>
/// Capabilities specific to the notebook document support.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentClientCapabilities
{
    /// <summary>
    /// Capabilities specific to notebook document synchronization
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("synchronization")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required NotebookDocumentSyncClientCapabilities Synchronization { get; set; }
}

/// <summary>
/// Notebook specific client capabilities.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentSyncClientCapabilities
{
    /// <summary>
    /// Whether implementation supports dynamic registration. If this is
    /// set to `true` the client supports the new
    /// `(TextDocumentRegistrationOptions & StaticRegistrationOptions)`
    /// return value for the corresponding server capability as well.
    /// </summary>
    [JsonPropertyName("dynamicRegistration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DynamicRegistration { get; set; }

    /// <summary>
    /// The client supports sending execution summary data per cell.
    /// </summary>
    [JsonPropertyName("executionSummarySupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ExecutionSummarySupport { get; set; }
}

public sealed class WindowClientCapabilities
{
    /// <summary>
    /// It indicates whether the client supports server initiated
    /// progress using the `window/workDoneProgress/create` request.
    /// 
    /// The capability also controls Whether client supports handling
    /// of progress notifications. If set servers are allowed to report a
    /// `workDoneProgress` property in the request specific server
    /// capabilities.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// Capabilities specific to the showMessage request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("showMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ShowMessageRequestClientCapabilities? ShowMessage { get; set; }

    /// <summary>
    /// Capabilities specific to the showDocument request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("showDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ShowDocumentClientCapabilities? ShowDocument { get; set; }
}

/// <summary>
/// Show message request client capabilities
/// </summary>
public sealed class ShowMessageRequestClientCapabilities
{
    public sealed class MessageActionItemCapabilities
    {
        /// <summary>
        /// Whether the client supports additional attributes which
        /// are preserved and send back to the server in the
        /// request's response.
        /// </summary>
        [JsonPropertyName("additionalPropertiesSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? AdditionalPropertiesSupport { get; set; }
    }

    /// <summary>
    /// Capabilities specific to the `MessageActionItem` type.
    /// </summary>
    [JsonPropertyName("messageActionItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ShowMessageRequestClientCapabilities.MessageActionItemCapabilities? MessageActionItem { get; set; }
}

/// <summary>
/// Client capabilities for the showDocument request.
/// 
/// @since 3.16.0
/// </summary>
public sealed class ShowDocumentClientCapabilities
{
    /// <summary>
    /// The client has support for the showDocument
    /// request.
    /// </summary>
    [JsonPropertyName("support")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean Support { get; set; }
}

/// <summary>
/// General client capabilities.
/// 
/// @since 3.16.0
/// </summary>
public sealed class GeneralClientCapabilities
{
    public sealed class StaleRequestSupportCapabilities
    {
        /// <summary>
        /// The client will actively cancel the request.
        /// </summary>
        [JsonPropertyName("cancel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.Boolean Cancel { get; set; }

        /// <summary>
        /// The list of requests for which the client
        /// will retry the request if it receives a
        /// response with error code `ContentModified`
        /// </summary>
        [JsonPropertyName("retryOnContentModified")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required IList<System.String> RetryOnContentModified { get; set; }
    }

    /// <summary>
    /// Client capability that signals how the client
    /// handles stale requests (e.g. a request
    /// for which the client will not process the response
    /// anymore since the information is outdated).
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("staleRequestSupport")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GeneralClientCapabilities.StaleRequestSupportCapabilities? StaleRequestSupport { get; set; }

    /// <summary>
    /// Client capabilities specific to regular expressions.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("regularExpressions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RegularExpressionsClientCapabilities? RegularExpressions { get; set; }

    /// <summary>
    /// Client capabilities specific to the client's markdown parser.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("markdown")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MarkdownClientCapabilities? Markdown { get; set; }

    /// <summary>
    /// The position encodings supported by the client. Client and server
    /// have to agree on the same position encoding to ensure that offsets
    /// (e.g. character position in a line) are interpreted the same on both
    /// sides.
    /// 
    /// To keep the protocol backwards compatible the following applies: if
    /// the value 'utf-16' is missing from the array of position encodings
    /// servers can assume that the client supports UTF-16. UTF-16 is
    /// therefore a mandatory encoding.
    /// 
    /// If omitted it defaults to ['utf-16'].
    /// 
    /// Implementation considerations: since the conversion from one encoding
    /// into another requires the content of the file / line the conversion
    /// is best done where the file is read which is usually on the server
    /// side.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("positionEncodings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<PositionEncodingKind>? PositionEncodings { get; set; }
}

/// <summary>
/// Client capabilities specific to regular expressions.
/// 
/// @since 3.16.0
/// </summary>
public sealed class RegularExpressionsClientCapabilities
{
    /// <summary>
    /// The engine's name.
    /// </summary>
    [JsonPropertyName("engine")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Engine { get; set; }

    /// <summary>
    /// The engine's version.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Version { get; set; }
}

/// <summary>
/// Client capabilities specific to the used markdown parser.
/// 
/// @since 3.16.0
/// </summary>
public sealed class MarkdownClientCapabilities
{
    /// <summary>
    /// The name of the parser.
    /// </summary>
    [JsonPropertyName("parser")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Parser { get; set; }

    /// <summary>
    /// The version of the parser.
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Version { get; set; }

    /// <summary>
    /// A list of HTML tags that the client allows / supports in
    /// Markdown.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("allowedTags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? AllowedTags { get; set; }
}

/// <summary>
/// A set of predefined position encoding kinds.
/// 
/// @since 3.17.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum PositionEncodingKind
{
    /// <summary>
    /// Character offsets count UTF-8 code units (e.g. bytes).
    /// </summary>
    [EnumMember(Value = "utf-8")]
    UTF8,
    /// <summary>
    /// Character offsets count UTF-16 code units.
    /// 
    /// This is the default and must always be supported
    /// by servers
    /// </summary>
    [EnumMember(Value = "utf-16")]
    UTF16,
    /// <summary>
    /// Character offsets count UTF-32 code units.
    /// 
    /// Implementation note: these are the same as Unicode codepoints,
    /// so this `PositionEncodingKind` may also be used for an
    /// encoding-agnostic representation of character offsets.
    /// </summary>
    [EnumMember(Value = "utf-32")]
    UTF32,
}

[JsonConverter(typeof(EnumValueConverter))]
public enum TraceValues
{
    /// <summary>
    /// Turn tracing off.
    /// </summary>
    [EnumMember(Value = "off")]
    Off,
    /// <summary>
    /// Trace messages only.
    /// </summary>
    [EnumMember(Value = "messages")]
    Messages,
    /// <summary>
    /// Verbose message tracing.
    /// </summary>
    [EnumMember(Value = "verbose")]
    Verbose,
}

public sealed class WorkspaceFoldersInitializeParams : IWorkspaceFoldersInitializeParams
{
    /// <summary>
    /// The workspace folders configured in the client when the server starts.
    /// 
    /// This property is only available if the client supports workspace folders.
    /// It can be `null` if the client supports workspace folders but none are
    /// configured.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("workspaceFolders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<WorkspaceFolder>? WorkspaceFolders { get; set; }
}

public interface IWorkspaceFoldersInitializeParams
{
    /// <summary>
    /// The workspace folders configured in the client when the server starts.
    /// 
    /// This property is only available if the client supports workspace folders.
    /// It can be `null` if the client supports workspace folders but none are
    /// configured.
    /// 
    /// @since 3.6.0
    /// </summary>
    [JsonPropertyName("workspaceFolders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<WorkspaceFolder>? WorkspaceFolders { get; }
}

/// <summary>
/// The result returned from an initialize request.
/// </summary>
public sealed class InitializeResult
{
    public sealed class ServerInfoResult
    {
        /// <summary>
        /// The name of the server as defined by the server.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.String Name { get; set; }

        /// <summary>
        /// The server's version as defined by the server.
        /// </summary>
        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.String? Version { get; set; }
    }

    /// <summary>
    /// The capabilities the language server provides.
    /// </summary>
    [JsonPropertyName("capabilities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ServerCapabilities Capabilities { get; set; }

    /// <summary>
    /// Information about the server.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("serverInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InitializeResult.ServerInfoResult? ServerInfo { get; set; }
}

/// <summary>
/// Defines the capabilities provided by a language
/// server.
/// </summary>
public sealed class ServerCapabilities
{
    public sealed class WorkspaceCapabilities
    {
        /// <summary>
        /// The server supports workspace folder.
        /// 
        /// @since 3.6.0
        /// </summary>
        [JsonPropertyName("workspaceFolders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public WorkspaceFoldersServerCapabilities? WorkspaceFolders { get; set; }

        /// <summary>
        /// The server is interested in notifications/requests for operations on files.
        /// 
        /// @since 3.16.0
        /// </summary>
        [JsonPropertyName("fileOperations")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FileOperationOptions? FileOperations { get; set; }
    }

    /// <summary>
    /// The position encoding the server picked from the encodings offered
    /// by the client via the client capability `general.positionEncodings`.
    /// 
    /// If the client didn't provide any position encodings the only valid
    /// value that a server can return is 'utf-16'.
    /// 
    /// If omitted it defaults to 'utf-16'.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("positionEncoding")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PositionEncodingKind? PositionEncoding { get; set; }

    /// <summary>
    /// Defines how text documents are synced. Is either a detailed structure
    /// defining each notification or for backwards compatibility the
    /// TextDocumentSyncKind number.
    /// </summary>
    [JsonPropertyName("textDocumentSync")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<TextDocumentSyncOptions, TextDocumentSyncKind>? TextDocumentSync { get; set; }

    /// <summary>
    /// Defines how notebook documents are synced.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("notebookDocumentSync")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<INotebookDocumentSyncOptions, NotebookDocumentSyncRegistrationOptions>? NotebookDocumentSync { get; set; }

    /// <summary>
    /// The server provides completion support.
    /// </summary>
    [JsonPropertyName("completionProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICompletionOptions? CompletionProvider { get; set; }

    /// <summary>
    /// The server provides hover support.
    /// </summary>
    [JsonPropertyName("hoverProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IHoverOptions>? HoverProvider { get; set; }

    /// <summary>
    /// The server provides signature help support.
    /// </summary>
    [JsonPropertyName("signatureHelpProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ISignatureHelpOptions? SignatureHelpProvider { get; set; }

    /// <summary>
    /// The server provides Goto Declaration support.
    /// </summary>
    [JsonPropertyName("declarationProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDeclarationOptions, DeclarationRegistrationOptions>? DeclarationProvider { get; set; }

    /// <summary>
    /// The server provides goto definition support.
    /// </summary>
    [JsonPropertyName("definitionProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDefinitionOptions>? DefinitionProvider { get; set; }

    /// <summary>
    /// The server provides Goto Type Definition support.
    /// </summary>
    [JsonPropertyName("typeDefinitionProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ITypeDefinitionOptions, TypeDefinitionRegistrationOptions>? TypeDefinitionProvider { get; set; }

    /// <summary>
    /// The server provides Goto Implementation support.
    /// </summary>
    [JsonPropertyName("implementationProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IImplementationOptions, ImplementationRegistrationOptions>? ImplementationProvider { get; set; }

    /// <summary>
    /// The server provides find references support.
    /// </summary>
    [JsonPropertyName("referencesProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IReferenceOptions>? ReferencesProvider { get; set; }

    /// <summary>
    /// The server provides document highlight support.
    /// </summary>
    [JsonPropertyName("documentHighlightProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDocumentHighlightOptions>? DocumentHighlightProvider { get; set; }

    /// <summary>
    /// The server provides document symbol support.
    /// </summary>
    [JsonPropertyName("documentSymbolProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDocumentSymbolOptions>? DocumentSymbolProvider { get; set; }

    /// <summary>
    /// The server provides code actions. CodeActionOptions may only be
    /// specified if the client states that it supports
    /// `codeActionLiteralSupport` in its initial `initialize` request.
    /// </summary>
    [JsonPropertyName("codeActionProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ICodeActionOptions>? CodeActionProvider { get; set; }

    /// <summary>
    /// The server provides code lens.
    /// </summary>
    [JsonPropertyName("codeLensProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICodeLensOptions? CodeLensProvider { get; set; }

    /// <summary>
    /// The server provides document link support.
    /// </summary>
    [JsonPropertyName("documentLinkProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDocumentLinkOptions? DocumentLinkProvider { get; set; }

    /// <summary>
    /// The server provides color provider support.
    /// </summary>
    [JsonPropertyName("colorProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDocumentColorOptions, DocumentColorRegistrationOptions>? ColorProvider { get; set; }

    /// <summary>
    /// The server provides workspace symbol support.
    /// </summary>
    [JsonPropertyName("workspaceSymbolProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IWorkspaceSymbolOptions>? WorkspaceSymbolProvider { get; set; }

    /// <summary>
    /// The server provides document formatting.
    /// </summary>
    [JsonPropertyName("documentFormattingProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDocumentFormattingOptions>? DocumentFormattingProvider { get; set; }

    /// <summary>
    /// The server provides document range formatting.
    /// </summary>
    [JsonPropertyName("documentRangeFormattingProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IDocumentRangeFormattingOptions>? DocumentRangeFormattingProvider { get; set; }

    /// <summary>
    /// The server provides document formatting on typing.
    /// </summary>
    [JsonPropertyName("documentOnTypeFormattingProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDocumentOnTypeFormattingOptions? DocumentOnTypeFormattingProvider { get; set; }

    /// <summary>
    /// The server provides rename support. RenameOptions may only be
    /// specified if the client states that it supports
    /// `prepareSupport` in its initial `initialize` request.
    /// </summary>
    [JsonPropertyName("renameProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IRenameOptions>? RenameProvider { get; set; }

    /// <summary>
    /// The server provides folding provider support.
    /// </summary>
    [JsonPropertyName("foldingRangeProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IFoldingRangeOptions, FoldingRangeRegistrationOptions>? FoldingRangeProvider { get; set; }

    /// <summary>
    /// The server provides selection range support.
    /// </summary>
    [JsonPropertyName("selectionRangeProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ISelectionRangeOptions, SelectionRangeRegistrationOptions>? SelectionRangeProvider { get; set; }

    /// <summary>
    /// The server provides execute command support.
    /// </summary>
    [JsonPropertyName("executeCommandProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IExecuteCommandOptions? ExecuteCommandProvider { get; set; }

    /// <summary>
    /// The server provides call hierarchy support.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("callHierarchyProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ICallHierarchyOptions, CallHierarchyRegistrationOptions>? CallHierarchyProvider { get; set; }

    /// <summary>
    /// The server provides linked editing range support.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("linkedEditingRangeProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ILinkedEditingRangeOptions, LinkedEditingRangeRegistrationOptions>? LinkedEditingRangeProvider { get; set; }

    /// <summary>
    /// The server provides semantic tokens support.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("semanticTokensProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<ISemanticTokensOptions, SemanticTokensRegistrationOptions>? SemanticTokensProvider { get; set; }

    /// <summary>
    /// The server provides moniker support.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("monikerProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IMonikerOptions, MonikerRegistrationOptions>? MonikerProvider { get; set; }

    /// <summary>
    /// The server provides type hierarchy support.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("typeHierarchyProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ITypeHierarchyOptions, TypeHierarchyRegistrationOptions>? TypeHierarchyProvider { get; set; }

    /// <summary>
    /// The server provides inline values.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("inlineValueProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IInlineValueOptions, InlineValueRegistrationOptions>? InlineValueProvider { get; set; }

    /// <summary>
    /// The server provides inlay hints.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("inlayHintProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, IInlayHintOptions, InlayHintRegistrationOptions>? InlayHintProvider { get; set; }

    /// <summary>
    /// The server has support for pull model diagnostics.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("diagnosticProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<IDiagnosticOptions, DiagnosticRegistrationOptions>? DiagnosticProvider { get; set; }

    /// <summary>
    /// Workspace specific server capabilities.
    /// </summary>
    [JsonPropertyName("workspace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ServerCapabilities.WorkspaceCapabilities? Workspace { get; set; }

    /// <summary>
    /// Experimental server capabilities.
    /// </summary>
    [JsonPropertyName("experimental")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Experimental { get; set; }
}

public sealed class TextDocumentSyncOptions
{
    /// <summary>
    /// Open and close notifications are sent to the server. If omitted open close notification should not
    /// be sent.
    /// </summary>
    [JsonPropertyName("openClose")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? OpenClose { get; set; }

    /// <summary>
    /// Change notifications are sent to the server. See TextDocumentSyncKind.None, TextDocumentSyncKind.Full
    /// and TextDocumentSyncKind.Incremental. If omitted it defaults to TextDocumentSyncKind.None.
    /// </summary>
    [JsonPropertyName("change")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TextDocumentSyncKind? Change { get; set; }

    /// <summary>
    /// If present will save notifications are sent to the server. If omitted the notification should not be
    /// sent.
    /// </summary>
    [JsonPropertyName("willSave")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillSave { get; set; }

    /// <summary>
    /// If present will save wait until requests are sent to the server. If omitted the request should not be
    /// sent.
    /// </summary>
    [JsonPropertyName("willSaveWaitUntil")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WillSaveWaitUntil { get; set; }

    /// <summary>
    /// If present save notifications are sent to the server. If omitted the notification should not be
    /// sent.
    /// </summary>
    [JsonPropertyName("save")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Boolean, ISaveOptions>? Save { get; set; }
}

/// <summary>
/// Defines how the host (editor) should sync
/// document changes to the language server.
/// </summary>
public enum TextDocumentSyncKind
{
    /// <summary>
    /// Documents should not be synced at all.
    /// </summary>
    None = 0,
    /// <summary>
    /// Documents are synced by always sending the full content
    /// of the document.
    /// </summary>
    Full = 1,
    /// <summary>
    /// Documents are synced by sending the full content on open.
    /// After that only incremental updates to the document are
    /// send.
    /// </summary>
    Incremental = 2,
}

/// <summary>
/// Save options.
/// </summary>
public sealed class SaveOptions : ISaveOptions
{
    /// <summary>
    /// The client is supposed to include the content on save.
    /// </summary>
    [JsonPropertyName("includeText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IncludeText { get; set; }
}

/// <summary>
/// Save options.
/// </summary>
public interface ISaveOptions
{
    /// <summary>
    /// The client is supposed to include the content on save.
    /// </summary>
    [JsonPropertyName("includeText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IncludeText { get; }
}

/// <summary>
/// Options specific to a notebook plus its cells
/// to be synced to the server.
/// 
/// If a selector provides a notebook document
/// filter but no cell selector all cells of a
/// matching notebook document will be synced.
/// 
/// If a selector provides no notebook document
/// filter but only a cell selector all notebook
/// document that contain at least one matching
/// cell will be synced.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentSyncOptions : INotebookDocumentSyncOptions
{
    public sealed class NotebookSelectorOptions
    {
        public sealed class NotebookOptions
        {
            /// <summary>
            /// The type of the enclosing notebook.
            /// </summary>
            [JsonPropertyName("notebookType")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public System.String? NotebookType { get; set; }

            /// <summary>
            /// A Uri {@link Uri.scheme scheme}, like `file` or `untitled`.
            /// </summary>
            [JsonPropertyName("scheme")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public System.String? Scheme { get; set; }

            /// <summary>
            /// A glob pattern.
            /// </summary>
            [JsonPropertyName("pattern")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public System.String? Pattern { get; set; }
        }

        public sealed class CellsOptions
        {
            [JsonPropertyName("language")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required System.String Language { get; set; }
        }

        /// <summary>
        /// The notebook to be synced If a string
        /// value is provided it matches against the
        /// notebook type. '*' matches every notebook.
        /// </summary>
        [JsonPropertyName("notebook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OneOf<System.String, NotebookDocumentSyncOptions.NotebookSelectorOptions.NotebookOptions>? Notebook { get; set; }

        /// <summary>
        /// The cells of the matching notebook to be synced.
        /// </summary>
        [JsonPropertyName("cells")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<NotebookDocumentSyncOptions.NotebookSelectorOptions.CellsOptions>? Cells { get; set; }
    }

    /// <summary>
    /// The notebooks to be synced
    /// </summary>
    [JsonPropertyName("notebookSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<NotebookDocumentSyncOptions.NotebookSelectorOptions> NotebookSelector { get; set; }

    /// <summary>
    /// Whether save notification should be forwarded to
    /// the server. Will only be honored if mode === `notebook`.
    /// </summary>
    [JsonPropertyName("save")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Save { get; set; }
}

/// <summary>
/// Options specific to a notebook plus its cells
/// to be synced to the server.
/// 
/// If a selector provides a notebook document
/// filter but no cell selector all cells of a
/// matching notebook document will be synced.
/// 
/// If a selector provides no notebook document
/// filter but only a cell selector all notebook
/// document that contain at least one matching
/// cell will be synced.
/// 
/// @since 3.17.0
/// </summary>
public interface INotebookDocumentSyncOptions
{
    /// <summary>
    /// The notebooks to be synced
    /// </summary>
    [JsonPropertyName("notebookSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public IList<NotebookDocumentSyncOptions.NotebookSelectorOptions> NotebookSelector { get; }

    /// <summary>
    /// Whether save notification should be forwarded to
    /// the server. Will only be honored if mode === `notebook`.
    /// </summary>
    [JsonPropertyName("save")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Save { get; }
}

/// <summary>
/// Registration options specific to a notebook.
/// 
/// @since 3.17.0
/// </summary>
public sealed class NotebookDocumentSyncRegistrationOptions : INotebookDocumentSyncOptions, IStaticRegistrationOptions
{
    /// <summary>
    /// The notebooks to be synced
    /// </summary>
    [JsonPropertyName("notebookSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<NotebookDocumentSyncOptions.NotebookSelectorOptions> NotebookSelector { get; set; }

    /// <summary>
    /// Whether save notification should be forwarded to
    /// the server. Will only be honored if mode === `notebook`.
    /// </summary>
    [JsonPropertyName("save")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Save { get; set; }

    /// <summary>
    /// The id used to register the request. The id can be used to deregister
    /// the request again. See also Registration#id.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Id { get; set; }
}

/// <summary>
/// Completion options.
/// </summary>
public sealed class CompletionOptions : ICompletionOptions, IWorkDoneProgressOptions
{
    public sealed class CompletionItemOptions
    {
        /// <summary>
        /// The server has support for completion item label
        /// details (see also `CompletionItemLabelDetails`) when
        /// receiving a completion item in a resolve call.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("labelDetailsSupport")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Boolean? LabelDetailsSupport { get; set; }
    }

    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// Most tools trigger completion request automatically without explicitly requesting
    /// it using a keyboard shortcut (e.g. Ctrl+Space). Typically they do so when the user
    /// starts to type an identifier. For example if the user types `c` in a JavaScript file
    /// code complete will automatically pop up present `console` besides others as a
    /// completion item. Characters that make up identifiers don't need to be listed here.
    /// 
    /// If code complete should automatically be trigger on characters not being valid inside
    /// an identifier (for example `.` in JavaScript) list them in `triggerCharacters`.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; set; }

    /// <summary>
    /// The list of all possible characters that commit a completion. This field can be used
    /// if clients don't support individual commit characters per completion item. See
    /// `ClientCapabilities.textDocument.completion.completionItem.commitCharactersSupport`
    /// 
    /// If a server provides both `allCommitCharacters` and commit characters on an individual
    /// completion item the ones on the completion item win.
    /// 
    /// @since 3.2.0
    /// </summary>
    [JsonPropertyName("allCommitCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? AllCommitCharacters { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a completion item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }

    /// <summary>
    /// The server supports the following `CompletionItem` specific
    /// capabilities.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("completionItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionOptions.CompletionItemOptions? CompletionItem { get; set; }
}

/// <summary>
/// Completion options.
/// </summary>
public interface ICompletionOptions
{
    /// <summary>
    /// Most tools trigger completion request automatically without explicitly requesting
    /// it using a keyboard shortcut (e.g. Ctrl+Space). Typically they do so when the user
    /// starts to type an identifier. For example if the user types `c` in a JavaScript file
    /// code complete will automatically pop up present `console` besides others as a
    /// completion item. Characters that make up identifiers don't need to be listed here.
    /// 
    /// If code complete should automatically be trigger on characters not being valid inside
    /// an identifier (for example `.` in JavaScript) list them in `triggerCharacters`.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; }

    /// <summary>
    /// The list of all possible characters that commit a completion. This field can be used
    /// if clients don't support individual commit characters per completion item. See
    /// `ClientCapabilities.textDocument.completion.completionItem.commitCharactersSupport`
    /// 
    /// If a server provides both `allCommitCharacters` and commit characters on an individual
    /// completion item the ones on the completion item win.
    /// 
    /// @since 3.2.0
    /// </summary>
    [JsonPropertyName("allCommitCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? AllCommitCharacters { get; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a completion item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }

    /// <summary>
    /// The server supports the following `CompletionItem` specific
    /// capabilities.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("completionItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionOptions.CompletionItemOptions? CompletionItem { get; }
}

/// <summary>
/// Hover options.
/// </summary>
public sealed class HoverOptions : IHoverOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Hover options.
/// </summary>
public interface IHoverOptions
{
}

/// <summary>
/// Server Capabilities for a {@link SignatureHelpRequest}.
/// </summary>
public sealed class SignatureHelpOptions : ISignatureHelpOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// List of characters that trigger signature help automatically.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; set; }

    /// <summary>
    /// List of characters that re-trigger signature help.
    /// 
    /// These trigger characters are only active when signature help is already showing. All trigger characters
    /// are also counted as re-trigger characters.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("retriggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? RetriggerCharacters { get; set; }
}

/// <summary>
/// Server Capabilities for a {@link SignatureHelpRequest}.
/// </summary>
public interface ISignatureHelpOptions
{
    /// <summary>
    /// List of characters that trigger signature help automatically.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; }

    /// <summary>
    /// List of characters that re-trigger signature help.
    /// 
    /// These trigger characters are only active when signature help is already showing. All trigger characters
    /// are also counted as re-trigger characters.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("retriggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? RetriggerCharacters { get; }
}

/// <summary>
/// Server Capabilities for a {@link DefinitionRequest}.
/// </summary>
public sealed class DefinitionOptions : IDefinitionOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Server Capabilities for a {@link DefinitionRequest}.
/// </summary>
public interface IDefinitionOptions
{
}

/// <summary>
/// Reference options.
/// </summary>
public sealed class ReferenceOptions : IReferenceOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Reference options.
/// </summary>
public interface IReferenceOptions
{
}

/// <summary>
/// Provider options for a {@link DocumentHighlightRequest}.
/// </summary>
public sealed class DocumentHighlightOptions : IDocumentHighlightOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentHighlightRequest}.
/// </summary>
public interface IDocumentHighlightOptions
{
}

/// <summary>
/// Provider options for a {@link DocumentSymbolRequest}.
/// </summary>
public sealed class DocumentSymbolOptions : IDocumentSymbolOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// A human-readable string that is shown when multiple outlines trees
    /// are shown for the same document.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Label { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentSymbolRequest}.
/// </summary>
public interface IDocumentSymbolOptions
{
    /// <summary>
    /// A human-readable string that is shown when multiple outlines trees
    /// are shown for the same document.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Label { get; }
}

/// <summary>
/// Provider options for a {@link CodeActionRequest}.
/// </summary>
public sealed class CodeActionOptions : ICodeActionOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// CodeActionKinds that this server may return.
    /// 
    /// The list of kinds may be generic, such as `CodeActionKind.Refactor`, or the server
    /// may list out every specific kind they provide.
    /// </summary>
    [JsonPropertyName("codeActionKinds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<CodeActionKind>? CodeActionKinds { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a code action.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// Provider options for a {@link CodeActionRequest}.
/// </summary>
public interface ICodeActionOptions
{
    /// <summary>
    /// CodeActionKinds that this server may return.
    /// 
    /// The list of kinds may be generic, such as `CodeActionKind.Refactor`, or the server
    /// may list out every specific kind they provide.
    /// </summary>
    [JsonPropertyName("codeActionKinds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<CodeActionKind>? CodeActionKinds { get; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a code action.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }
}

/// <summary>
/// Code Lens provider options of a {@link CodeLensRequest}.
/// </summary>
public sealed class CodeLensOptions : ICodeLensOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// Code lens has a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// Code Lens provider options of a {@link CodeLensRequest}.
/// </summary>
public interface ICodeLensOptions
{
    /// <summary>
    /// Code lens has a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }
}

/// <summary>
/// Provider options for a {@link DocumentLinkRequest}.
/// </summary>
public sealed class DocumentLinkOptions : IDocumentLinkOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// Document links have a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentLinkRequest}.
/// </summary>
public interface IDocumentLinkOptions
{
    /// <summary>
    /// Document links have a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }
}

/// <summary>
/// Server capabilities for a {@link WorkspaceSymbolRequest}.
/// </summary>
public sealed class WorkspaceSymbolOptions : IWorkspaceSymbolOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a workspace symbol.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// Server capabilities for a {@link WorkspaceSymbolRequest}.
/// </summary>
public interface IWorkspaceSymbolOptions
{
    /// <summary>
    /// The server provides support to resolve additional
    /// information for a workspace symbol.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; }
}

/// <summary>
/// Provider options for a {@link DocumentFormattingRequest}.
/// </summary>
public sealed class DocumentFormattingOptions : IDocumentFormattingOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentFormattingRequest}.
/// </summary>
public interface IDocumentFormattingOptions
{
}

/// <summary>
/// Provider options for a {@link DocumentRangeFormattingRequest}.
/// </summary>
public sealed class DocumentRangeFormattingOptions : IDocumentRangeFormattingOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentRangeFormattingRequest}.
/// </summary>
public interface IDocumentRangeFormattingOptions
{
}

/// <summary>
/// Provider options for a {@link DocumentOnTypeFormattingRequest}.
/// </summary>
public sealed class DocumentOnTypeFormattingOptions : IDocumentOnTypeFormattingOptions
{
    /// <summary>
    /// A character on which formatting should be triggered, like `{`.
    /// </summary>
    [JsonPropertyName("firstTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String FirstTriggerCharacter { get; set; }

    /// <summary>
    /// More trigger characters.
    /// </summary>
    [JsonPropertyName("moreTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? MoreTriggerCharacter { get; set; }
}

/// <summary>
/// Provider options for a {@link DocumentOnTypeFormattingRequest}.
/// </summary>
public interface IDocumentOnTypeFormattingOptions
{
    /// <summary>
    /// A character on which formatting should be triggered, like `{`.
    /// </summary>
    [JsonPropertyName("firstTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String FirstTriggerCharacter { get; }

    /// <summary>
    /// More trigger characters.
    /// </summary>
    [JsonPropertyName("moreTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? MoreTriggerCharacter { get; }
}

/// <summary>
/// Provider options for a {@link RenameRequest}.
/// </summary>
public sealed class RenameOptions : IRenameOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// Renames should be checked and tested before being executed.
    /// 
    /// @since version 3.12.0
    /// </summary>
    [JsonPropertyName("prepareProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PrepareProvider { get; set; }
}

/// <summary>
/// Provider options for a {@link RenameRequest}.
/// </summary>
public interface IRenameOptions
{
    /// <summary>
    /// Renames should be checked and tested before being executed.
    /// 
    /// @since version 3.12.0
    /// </summary>
    [JsonPropertyName("prepareProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PrepareProvider { get; }
}

/// <summary>
/// The server capabilities of a {@link ExecuteCommandRequest}.
/// </summary>
public sealed class ExecuteCommandOptions : IExecuteCommandOptions, IWorkDoneProgressOptions
{
    [JsonPropertyName("workDoneProgress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? WorkDoneProgress { get; set; }

    /// <summary>
    /// The commands to be executed on the server
    /// </summary>
    [JsonPropertyName("commands")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> Commands { get; set; }
}

/// <summary>
/// The server capabilities of a {@link ExecuteCommandRequest}.
/// </summary>
public interface IExecuteCommandOptions
{
    /// <summary>
    /// The commands to be executed on the server
    /// </summary>
    [JsonPropertyName("commands")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public IList<System.String> Commands { get; }
}

public sealed class WorkspaceFoldersServerCapabilities
{
    /// <summary>
    /// The server has support for workspace folders
    /// </summary>
    [JsonPropertyName("supported")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Supported { get; set; }

    /// <summary>
    /// Whether the server wants to receive workspace folder
    /// change notifications.
    /// 
    /// If a string is provided the string is treated as an ID
    /// under which the notification is registered on the client
    /// side. The ID can be used to unregister for these events
    /// using the `client/unregisterCapability` request.
    /// </summary>
    [JsonPropertyName("changeNotifications")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, System.Boolean>? ChangeNotifications { get; set; }
}

/// <summary>
/// Options for notifications/requests for user operations on files.
/// 
/// @since 3.16.0
/// </summary>
public sealed class FileOperationOptions
{
    /// <summary>
    /// The server is interested in receiving didCreateFiles notifications.
    /// </summary>
    [JsonPropertyName("didCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? DidCreate { get; set; }

    /// <summary>
    /// The server is interested in receiving willCreateFiles requests.
    /// </summary>
    [JsonPropertyName("willCreate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? WillCreate { get; set; }

    /// <summary>
    /// The server is interested in receiving didRenameFiles notifications.
    /// </summary>
    [JsonPropertyName("didRename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? DidRename { get; set; }

    /// <summary>
    /// The server is interested in receiving willRenameFiles requests.
    /// </summary>
    [JsonPropertyName("willRename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? WillRename { get; set; }

    /// <summary>
    /// The server is interested in receiving didDeleteFiles file notifications.
    /// </summary>
    [JsonPropertyName("didDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? DidDelete { get; set; }

    /// <summary>
    /// The server is interested in receiving willDeleteFiles file requests.
    /// </summary>
    [JsonPropertyName("willDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileOperationRegistrationOptions? WillDelete { get; set; }
}

/// <summary>
/// The data type of the ResponseError if the
/// initialize request fails.
/// </summary>
public sealed class InitializeError
{
    /// <summary>
    /// Indicates whether the client execute the following retry logic:
    /// (1) show the message provided by the ResponseError to the user
    /// (2) user selects retry or cancel
    /// (3) if user selected retry the initialize method is sent again.
    /// </summary>
    [JsonPropertyName("retry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean Retry { get; set; }
}

public sealed class InitializedParams
{
}

/// <summary>
/// The parameters of a change configuration notification.
/// </summary>
public sealed class DidChangeConfigurationParams
{
    /// <summary>
    /// The actual changed settings
    /// </summary>
    [JsonPropertyName("settings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Text.Json.JsonElement Settings { get; set; }
}

public sealed class DidChangeConfigurationRegistrationOptions
{
    [JsonPropertyName("section")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, IList<System.String>>? Section { get; set; }
}

/// <summary>
/// The parameters of a notification message.
/// </summary>
public sealed class ShowMessageParams
{
    /// <summary>
    /// The message type. See {@link MessageType}
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required MessageType Type { get; set; }

    /// <summary>
    /// The actual message.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }
}

/// <summary>
/// The message type
/// </summary>
public enum MessageType
{
    /// <summary>
    /// An error message.
    /// </summary>
    Error = 1,
    /// <summary>
    /// A warning message.
    /// </summary>
    Warning = 2,
    /// <summary>
    /// An information message.
    /// </summary>
    Info = 3,
    /// <summary>
    /// A log message.
    /// </summary>
    Log = 4,
}

public sealed class ShowMessageRequestParams
{
    /// <summary>
    /// The message type. See {@link MessageType}
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required MessageType Type { get; set; }

    /// <summary>
    /// The actual message.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }

    /// <summary>
    /// The message action items to present.
    /// </summary>
    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<MessageActionItem>? Actions { get; set; }
}

public sealed class MessageActionItem
{
    /// <summary>
    /// A short title like 'Retry', 'Open Log' etc.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Title { get; set; }
}

/// <summary>
/// The log message parameters.
/// </summary>
public sealed class LogMessageParams
{
    /// <summary>
    /// The message type. See {@link MessageType}
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required MessageType Type { get; set; }

    /// <summary>
    /// The actual message.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }
}

/// <summary>
/// The parameters sent in an open text document notification
/// </summary>
public sealed class DidOpenTextDocumentParams
{
    /// <summary>
    /// The document that was opened.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TextDocumentItem TextDocument { get; set; }
}

/// <summary>
/// The change text document notification's parameters.
/// </summary>
public sealed class DidChangeTextDocumentParams
{
    /// <summary>
    /// The document that did change. The version number points
    /// to the version after all provided content changes have
    /// been applied.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required VersionedTextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The actual content changes. The content changes describe single state changes
    /// to the document. So if there are two content changes c1 (at array index 0) and
    /// c2 (at array index 1) for a document in state S then c1 moves the document from
    /// S to S' and c2 from S' to S''. So c1 is computed on the state S and c2 is computed
    /// on the state S'.
    /// 
    /// To mirror the content of a document using change events use the following approach:
    /// - start with the same initial content
    /// - apply the 'textDocument/didChange' notifications in the order you receive them.
    /// - apply the `TextDocumentContentChangeEvent`s in a single notification in the order
    ///   you receive them.
    /// </summary>
    [JsonPropertyName("contentChanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<TextDocumentContentChangeEvent> ContentChanges { get; set; }
}

/// <summary>
/// Describe options to be used when registered for text document change events.
/// </summary>
public sealed class TextDocumentChangeRegistrationOptions : ITextDocumentRegistrationOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// How documents are synced to the server.
    /// </summary>
    [JsonPropertyName("syncKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TextDocumentSyncKind SyncKind { get; set; }
}

/// <summary>
/// The parameters sent in a close text document notification
/// </summary>
public sealed class DidCloseTextDocumentParams
{
    /// <summary>
    /// The document that was closed.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// The parameters sent in a save text document notification
/// </summary>
public sealed class DidSaveTextDocumentParams
{
    /// <summary>
    /// The document that was saved.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// Optional the content when saved. Depends on the includeText value
    /// when the save notification was requested.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Text { get; set; }
}

/// <summary>
/// Save registration options.
/// </summary>
public sealed class TextDocumentSaveRegistrationOptions : ITextDocumentRegistrationOptions, ISaveOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// The client is supposed to include the content on save.
    /// </summary>
    [JsonPropertyName("includeText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IncludeText { get; set; }
}

/// <summary>
/// The parameters sent in a will save text document notification.
/// </summary>
public sealed class WillSaveTextDocumentParams
{
    /// <summary>
    /// The document that will be saved.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The 'TextDocumentSaveReason'.
    /// </summary>
    [JsonPropertyName("reason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TextDocumentSaveReason Reason { get; set; }
}

/// <summary>
/// Represents reasons why a text document is saved.
/// </summary>
public enum TextDocumentSaveReason
{
    /// <summary>
    /// Manually triggered, e.g. by the user pressing save, by starting debugging,
    /// or by an API call.
    /// </summary>
    Manual = 1,
    /// <summary>
    /// Automatic after a delay.
    /// </summary>
    AfterDelay = 2,
    /// <summary>
    /// When the editor lost focus.
    /// </summary>
    FocusOut = 3,
}

/// <summary>
/// The watched files change notification's parameters.
/// </summary>
public sealed class DidChangeWatchedFilesParams
{
    /// <summary>
    /// The actual file events.
    /// </summary>
    [JsonPropertyName("changes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<FileEvent> Changes { get; set; }
}

/// <summary>
/// An event describing a file change.
/// </summary>
public sealed class FileEvent
{
    /// <summary>
    /// The file's uri.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// The change type.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required FileChangeType Type { get; set; }
}

/// <summary>
/// The file event type
/// </summary>
public enum FileChangeType
{
    /// <summary>
    /// The file got created.
    /// </summary>
    Created = 1,
    /// <summary>
    /// The file got changed.
    /// </summary>
    Changed = 2,
    /// <summary>
    /// The file got deleted.
    /// </summary>
    Deleted = 3,
}

/// <summary>
/// Describe options to be used when registered for text document change events.
/// </summary>
public sealed class DidChangeWatchedFilesRegistrationOptions
{
    /// <summary>
    /// The watchers to register.
    /// </summary>
    [JsonPropertyName("watchers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<FileSystemWatcher> Watchers { get; set; }
}

public sealed class FileSystemWatcher
{
    /// <summary>
    /// The glob pattern to watch. See {@link GlobPattern glob pattern} for more detail.
    /// 
    /// @since 3.17.0 support for relative patterns.
    /// </summary>
    [JsonPropertyName("globPattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.String, RelativePattern> GlobPattern { get; set; }

    /// <summary>
    /// The kind of events of interest. If omitted it defaults
    /// to WatchKind.Create | WatchKind.Change | WatchKind.Delete
    /// which is 7.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WatchKind? Kind { get; set; }
}

/// <summary>
/// A relative pattern is a helper to construct glob patterns that are matched
/// relatively to a base URI. The common value for a `baseUri` is a workspace
/// folder root, but it can be another absolute URI as well.
/// 
/// @since 3.17.0
/// </summary>
public sealed class RelativePattern
{
    /// <summary>
    /// A workspace folder or a base URI to which this pattern will be matched
    /// against relatively.
    /// </summary>
    [JsonPropertyName("baseUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<WorkspaceFolder, System.Uri> BaseUri { get; set; }

    /// <summary>
    /// The actual glob pattern;
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Pattern { get; set; }
}

public enum WatchKind
{
    /// <summary>
    /// Interested in create events.
    /// </summary>
    Create = 1,
    /// <summary>
    /// Interested in change events
    /// </summary>
    Change = 2,
    /// <summary>
    /// Interested in delete events
    /// </summary>
    Delete = 4,
}

/// <summary>
/// The publish diagnostic notification's parameters.
/// </summary>
public sealed class PublishDiagnosticsParams
{
    /// <summary>
    /// The URI for which diagnostic information is reported.
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri Uri { get; set; }

    /// <summary>
    /// Optional the version number of the document the diagnostics are published for.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Int32? Version { get; set; }

    /// <summary>
    /// An array of diagnostic information items.
    /// </summary>
    [JsonPropertyName("diagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Diagnostic> Diagnostics { get; set; }
}

/// <summary>
/// Completion parameters
/// </summary>
public sealed class CompletionParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The completion context. This is only available it the client specifies
    /// to send this using the client capability `textDocument.completion.contextSupport === true`
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionContext? Context { get; set; }
}

/// <summary>
/// Contains additional information about the context in which a completion request is triggered.
/// </summary>
public sealed class CompletionContext
{
    /// <summary>
    /// How the completion was triggered.
    /// </summary>
    [JsonPropertyName("triggerKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CompletionTriggerKind TriggerKind { get; set; }

    /// <summary>
    /// The trigger character (a single character) that has trigger code complete.
    /// Is undefined if `triggerKind !== CompletionTriggerKind.TriggerCharacter`
    /// </summary>
    [JsonPropertyName("triggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? TriggerCharacter { get; set; }
}

/// <summary>
/// How a completion was triggered
/// </summary>
public enum CompletionTriggerKind
{
    /// <summary>
    /// Completion was triggered by typing an identifier (24x7 code
    /// complete), manual invocation (e.g Ctrl+Space) or via API.
    /// </summary>
    Invoked = 1,
    /// <summary>
    /// Completion was triggered by a trigger character specified by
    /// the `triggerCharacters` properties of the `CompletionRegistrationOptions`.
    /// </summary>
    TriggerCharacter = 2,
    /// <summary>
    /// Completion was re-triggered as current completion list is incomplete
    /// </summary>
    TriggerForIncompleteCompletions = 3,
}

/// <summary>
/// A completion item represents a text snippet that is
/// proposed to complete text that is being typed.
/// </summary>
public sealed class CompletionItem
{
    /// <summary>
    /// The label of this completion item.
    /// 
    /// The label property is also by default the text that
    /// is inserted when selecting this completion.
    /// 
    /// If label details are provided the label itself should
    /// be an unqualified name of the completion item.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Label { get; set; }

    /// <summary>
    /// Additional details for the label
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("labelDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionItemLabelDetails? LabelDetails { get; set; }

    /// <summary>
    /// The kind of this completion item. Based of the kind
    /// an icon is chosen by the editor.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionItemKind? Kind { get; set; }

    /// <summary>
    /// Tags for this completion item.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<CompletionItemTag>? Tags { get; set; }

    /// <summary>
    /// A human-readable string with additional information
    /// about this item, like type or symbol information.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Detail { get; set; }

    /// <summary>
    /// A human-readable string that represents a doc-comment.
    /// </summary>
    [JsonPropertyName("documentation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, MarkupContent>? Documentation { get; set; }

    /// <summary>
    /// Indicates if this item is deprecated.
    /// @deprecated Use `tags` instead.
    /// </summary>
    [Obsolete("Use `tags` instead.")]
    [JsonPropertyName("deprecated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Deprecated { get; set; }

    /// <summary>
    /// Select this item when showing.
    /// 
    /// *Note* that only one completion item can be selected and that the
    /// tool / client decides which item that is. The rule is that the *first*
    /// item of those that match best is selected.
    /// </summary>
    [JsonPropertyName("preselect")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Preselect { get; set; }

    /// <summary>
    /// A string that should be used when comparing this item
    /// with other items. When `falsy` the {@link CompletionItem.label label}
    /// is used.
    /// </summary>
    [JsonPropertyName("sortText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? SortText { get; set; }

    /// <summary>
    /// A string that should be used when filtering a set of
    /// completion items. When `falsy` the {@link CompletionItem.label label}
    /// is used.
    /// </summary>
    [JsonPropertyName("filterText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? FilterText { get; set; }

    /// <summary>
    /// A string that should be inserted into a document when selecting
    /// this completion. When `falsy` the {@link CompletionItem.label label}
    /// is used.
    /// 
    /// The `insertText` is subject to interpretation by the client side.
    /// Some tools might not take the string literally. For example
    /// VS Code when code complete is requested in this example
    /// `con<cursor position>` and a completion item with an `insertText` of
    /// `console` is provided it will only insert `sole`. Therefore it is
    /// recommended to use `textEdit` instead since it avoids additional client
    /// side interpretation.
    /// </summary>
    [JsonPropertyName("insertText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? InsertText { get; set; }

    /// <summary>
    /// The format of the insert text. The format applies to both the
    /// `insertText` property and the `newText` property of a provided
    /// `textEdit`. If omitted defaults to `InsertTextFormat.PlainText`.
    /// 
    /// Please note that the insertTextFormat doesn't apply to
    /// `additionalTextEdits`.
    /// </summary>
    [JsonPropertyName("insertTextFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InsertTextFormat? InsertTextFormat { get; set; }

    /// <summary>
    /// How whitespace and indentation is handled during completion
    /// item insertion. If not provided the clients default value depends on
    /// the `textDocument.completion.insertTextMode` client capability.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("insertTextMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public InsertTextMode? InsertTextMode { get; set; }

    /// <summary>
    /// An {@link TextEdit edit} which is applied to a document when selecting
    /// this completion. When an edit is provided the value of
    /// {@link CompletionItem.insertText insertText} is ignored.
    /// 
    /// Most editors support two different operations when accepting a completion
    /// item. One is to insert a completion text and the other is to replace an
    /// existing text with a completion text. Since this can usually not be
    /// predetermined by a server it can report both ranges. Clients need to
    /// signal support for `InsertReplaceEdits` via the
    /// `textDocument.completion.insertReplaceSupport` client capability
    /// property.
    /// 
    /// *Note 1:* The text edit's range as well as both ranges from an insert
    /// replace edit must be a [single line] and they must contain the position
    /// at which completion has been requested.
    /// *Note 2:* If an `InsertReplaceEdit` is returned the edit's insert range
    /// must be a prefix of the edit's replace range, that means it must be
    /// contained and starting at the same position.
    /// 
    /// @since 3.16.0 additional type `InsertReplaceEdit`
    /// </summary>
    [JsonPropertyName("textEdit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<ITextEdit, InsertReplaceEdit>? TextEdit { get; set; }

    /// <summary>
    /// The edit text used if the completion item is part of a CompletionList and
    /// CompletionList defines an item default for the text edit range.
    /// 
    /// Clients will only honor this property if they opt into completion list
    /// item defaults using the capability `completionList.itemDefaults`.
    /// 
    /// If not provided and a list's default range is provided the label
    /// property is used as a text.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("textEditText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? TextEditText { get; set; }

    /// <summary>
    /// An optional array of additional {@link TextEdit text edits} that are applied when
    /// selecting this completion. Edits must not overlap (including the same insert position)
    /// with the main {@link CompletionItem.textEdit edit} nor with themselves.
    /// 
    /// Additional text edits should be used to change text unrelated to the current cursor position
    /// (for example adding an import statement at the top of the file if the completion item will
    /// insert an unqualified type).
    /// </summary>
    [JsonPropertyName("additionalTextEdits")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ITextEdit>? AdditionalTextEdits { get; set; }

    /// <summary>
    /// An optional set of characters that when pressed while this completion is active will accept it first and
    /// then type that character. *Note* that all commit characters should have `length=1` and that superfluous
    /// characters will be ignored.
    /// </summary>
    [JsonPropertyName("commitCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? CommitCharacters { get; set; }

    /// <summary>
    /// An optional {@link Command command} that is executed *after* inserting this completion. *Note* that
    /// additional modifications to the current document should be described with the
    /// {@link CompletionItem.additionalTextEdits additionalTextEdits}-property.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Command? Command { get; set; }

    /// <summary>
    /// A data entry field that is preserved on a completion item between a
    /// {@link CompletionRequest} and a {@link CompletionResolveRequest}.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Additional details for a completion item label.
/// 
/// @since 3.17.0
/// </summary>
public sealed class CompletionItemLabelDetails
{
    /// <summary>
    /// An optional string which is rendered less prominently directly after {@link CompletionItem.label label},
    /// without any spacing. Should be used for function signatures and type annotations.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Detail { get; set; }

    /// <summary>
    /// An optional string which is rendered less prominently after {@link CompletionItem.detail}. Should be used
    /// for fully qualified names and file paths.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Description { get; set; }
}

/// <summary>
/// Defines whether the insert text in a completion item should be interpreted as
/// plain text or a snippet.
/// </summary>
public enum InsertTextFormat
{
    /// <summary>
    /// The primary text to be inserted is treated as a plain string.
    /// </summary>
    PlainText = 1,
    /// <summary>
    /// The primary text to be inserted is treated as a snippet.
    /// 
    /// A snippet can define tab stops and placeholders with `$1`, `$2`
    /// and `${3:foo}`. `$0` defines the final tab stop, it defaults to
    /// the end of the snippet. Placeholders with equal identifiers are linked,
    /// that is typing in one will update others too.
    /// 
    /// See also: https://microsoft.github.io/language-server-protocol/specifications/specification-current/#snippet_syntax
    /// </summary>
    Snippet = 2,
}

/// <summary>
/// A special text edit to provide an insert and a replace operation.
/// 
/// @since 3.16.0
/// </summary>
public sealed class InsertReplaceEdit
{
    /// <summary>
    /// The string to be inserted.
    /// </summary>
    [JsonPropertyName("newText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NewText { get; set; }

    /// <summary>
    /// The range if the insert is requested
    /// </summary>
    [JsonPropertyName("insert")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Insert { get; set; }

    /// <summary>
    /// The range if the replace is requested.
    /// </summary>
    [JsonPropertyName("replace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Replace { get; set; }
}

/// <summary>
/// Represents a collection of {@link CompletionItem completion items} to be presented
/// in the editor.
/// </summary>
public sealed class CompletionList
{
    public sealed class ItemDefaultsList
    {
        public sealed class EditRangeList
        {
            [JsonPropertyName("insert")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required Range Insert { get; set; }

            [JsonPropertyName("replace")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            [JsonRequired]
            public required Range Replace { get; set; }
        }

        /// <summary>
        /// A default commit character set.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("commitCharacters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<System.String>? CommitCharacters { get; set; }

        /// <summary>
        /// A default edit range.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("editRange")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OneOf<Range, CompletionList.ItemDefaultsList.EditRangeList>? EditRange { get; set; }

        /// <summary>
        /// A default insert text format.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("insertTextFormat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public InsertTextFormat? InsertTextFormat { get; set; }

        /// <summary>
        /// A default insert text mode.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("insertTextMode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public InsertTextMode? InsertTextMode { get; set; }

        /// <summary>
        /// A default data value.
        /// 
        /// @since 3.17.0
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public System.Text.Json.JsonElement? Data { get; set; }
    }

    /// <summary>
    /// This list it not complete. Further typing results in recomputing this list.
    /// 
    /// Recomputed lists have all their items replaced (not appended) in the
    /// incomplete completion sessions.
    /// </summary>
    [JsonPropertyName("isIncomplete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean IsIncomplete { get; set; }

    /// <summary>
    /// In many cases the items of an actual completion result share the same
    /// value for properties like `commitCharacters` or the range of a text
    /// edit. A completion list can therefore define item defaults which will
    /// be used if a completion item itself doesn't specify the value.
    /// 
    /// If a completion list specifies a default value and a completion item
    /// also specifies a corresponding value the one from the item is used.
    /// 
    /// Servers are only allowed to return default values if the client
    /// signals support for this via the `completionList.itemDefaults`
    /// capability.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("itemDefaults")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionList.ItemDefaultsList? ItemDefaults { get; set; }

    /// <summary>
    /// The completion items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<CompletionItem> Items { get; set; }
}

/// <summary>
/// Registration options for a {@link CompletionRequest}.
/// </summary>
public sealed class CompletionRegistrationOptions : ITextDocumentRegistrationOptions, ICompletionOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// Most tools trigger completion request automatically without explicitly requesting
    /// it using a keyboard shortcut (e.g. Ctrl+Space). Typically they do so when the user
    /// starts to type an identifier. For example if the user types `c` in a JavaScript file
    /// code complete will automatically pop up present `console` besides others as a
    /// completion item. Characters that make up identifiers don't need to be listed here.
    /// 
    /// If code complete should automatically be trigger on characters not being valid inside
    /// an identifier (for example `.` in JavaScript) list them in `triggerCharacters`.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; set; }

    /// <summary>
    /// The list of all possible characters that commit a completion. This field can be used
    /// if clients don't support individual commit characters per completion item. See
    /// `ClientCapabilities.textDocument.completion.completionItem.commitCharactersSupport`
    /// 
    /// If a server provides both `allCommitCharacters` and commit characters on an individual
    /// completion item the ones on the completion item win.
    /// 
    /// @since 3.2.0
    /// </summary>
    [JsonPropertyName("allCommitCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? AllCommitCharacters { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a completion item.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }

    /// <summary>
    /// The server supports the following `CompletionItem` specific
    /// capabilities.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("completionItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompletionOptions.CompletionItemOptions? CompletionItem { get; set; }
}

/// <summary>
/// Parameters for a {@link HoverRequest}.
/// </summary>
public sealed class HoverParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

/// <summary>
/// The result of a hover request.
/// </summary>
public sealed class Hover
{
    public sealed class ContentsHover
    {
        [JsonPropertyName("language")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.String Language { get; set; }

        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.String Value { get; set; }
    }

    /// <summary>
    /// The hover's content
    /// </summary>
    [JsonPropertyName("contents")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<MarkupContent, IList<MarkedString>, System.String, Hover.ContentsHover> Contents { get; set; }

    /// <summary>
    /// An optional range inside the text document that is used to
    /// visualize the hover, e.g. by changing the background color.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Range? Range { get; set; }
}

public sealed class MarkedString
{
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Language { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Value { get; set; }
}

/// <summary>
/// Registration options for a {@link HoverRequest}.
/// </summary>
public sealed class HoverRegistrationOptions : ITextDocumentRegistrationOptions, IHoverOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// Parameters for a {@link SignatureHelpRequest}.
/// </summary>
public sealed class SignatureHelpParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The signature help context. This is only available if the client specifies
    /// to send this using the client capability `textDocument.signatureHelp.contextSupport === true`
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SignatureHelpContext? Context { get; set; }
}

/// <summary>
/// Additional information about the context in which a signature help request was triggered.
/// 
/// @since 3.15.0
/// </summary>
public sealed class SignatureHelpContext
{
    /// <summary>
    /// Action that caused signature help to be triggered.
    /// </summary>
    [JsonPropertyName("triggerKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SignatureHelpTriggerKind TriggerKind { get; set; }

    /// <summary>
    /// Character that caused signature help to be triggered.
    /// 
    /// This is undefined when `triggerKind !== SignatureHelpTriggerKind.TriggerCharacter`
    /// </summary>
    [JsonPropertyName("triggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? TriggerCharacter { get; set; }

    /// <summary>
    /// `true` if signature help was already showing when it was triggered.
    /// 
    /// Retriggers occurs when the signature help is already active and can be caused by actions such as
    /// typing a trigger character, a cursor move, or document content changes.
    /// </summary>
    [JsonPropertyName("isRetrigger")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean IsRetrigger { get; set; }

    /// <summary>
    /// The currently active `SignatureHelp`.
    /// 
    /// The `activeSignatureHelp` has its `SignatureHelp.activeSignature` field updated based on
    /// the user navigating through available signatures.
    /// </summary>
    [JsonPropertyName("activeSignatureHelp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SignatureHelp? ActiveSignatureHelp { get; set; }
}

/// <summary>
/// How a signature help was triggered.
/// 
/// @since 3.15.0
/// </summary>
public enum SignatureHelpTriggerKind
{
    /// <summary>
    /// Signature help was invoked manually by the user or by a command.
    /// </summary>
    Invoked = 1,
    /// <summary>
    /// Signature help was triggered by a trigger character.
    /// </summary>
    TriggerCharacter = 2,
    /// <summary>
    /// Signature help was triggered by the cursor moving or by the document content changing.
    /// </summary>
    ContentChange = 3,
}

/// <summary>
/// Signature help represents the signature of something
/// callable. There can be multiple signature but only one
/// active and only one active parameter.
/// </summary>
public sealed class SignatureHelp
{
    /// <summary>
    /// One or more signatures.
    /// </summary>
    [JsonPropertyName("signatures")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<SignatureInformation> Signatures { get; set; }

    /// <summary>
    /// The active signature. If omitted or the value lies outside the
    /// range of `signatures` the value defaults to zero or is ignored if
    /// the `SignatureHelp` has no signatures.
    /// 
    /// Whenever possible implementors should make an active decision about
    /// the active signature and shouldn't rely on a default value.
    /// 
    /// In future version of the protocol this property might become
    /// mandatory to better express this.
    /// </summary>
    [JsonPropertyName("activeSignature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? ActiveSignature { get; set; }

    /// <summary>
    /// The active parameter of the active signature. If omitted or the value
    /// lies outside the range of `signatures[activeSignature].parameters`
    /// defaults to 0 if the active signature has parameters. If
    /// the active signature has no parameters it is ignored.
    /// In future version of the protocol this property might become
    /// mandatory to better express the active parameter if the
    /// active signature does have any.
    /// </summary>
    [JsonPropertyName("activeParameter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? ActiveParameter { get; set; }
}

/// <summary>
/// Represents the signature of something callable. A signature
/// can have a label, like a function-name, a doc-comment, and
/// a set of parameters.
/// </summary>
public sealed class SignatureInformation
{
    /// <summary>
    /// The label of this signature. Will be shown in
    /// the UI.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Label { get; set; }

    /// <summary>
    /// The human-readable doc-comment of this signature. Will be shown
    /// in the UI but can be omitted.
    /// </summary>
    [JsonPropertyName("documentation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, MarkupContent>? Documentation { get; set; }

    /// <summary>
    /// The parameters of this signature.
    /// </summary>
    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<ParameterInformation>? Parameters { get; set; }

    /// <summary>
    /// The index of the active parameter.
    /// 
    /// If provided, this is used in place of `SignatureHelp.activeParameter`.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("activeParameter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? ActiveParameter { get; set; }
}

/// <summary>
/// Represents a parameter of a callable-signature. A parameter can
/// have a label and a doc-comment.
/// </summary>
public sealed class ParameterInformation
{
    /// <summary>
    /// The label of this parameter information.
    /// 
    /// Either a string or an inclusive start and exclusive end offsets within its containing
    /// signature label. (see SignatureInformation.label). The offsets are based on a UTF-16
    /// string representation as `Position` and `Range` does.
    /// 
    /// *Note*: a label of type string should be a substring of its containing signature label.
    /// Its intended use case is to highlight the parameter label part in the `SignatureInformation.label`.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.String, (System.UInt32, System.UInt32)> Label { get; set; }

    /// <summary>
    /// The human-readable doc-comment of this parameter. Will be shown
    /// in the UI but can be omitted.
    /// </summary>
    [JsonPropertyName("documentation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.String, MarkupContent>? Documentation { get; set; }
}

/// <summary>
/// Registration options for a {@link SignatureHelpRequest}.
/// </summary>
public sealed class SignatureHelpRegistrationOptions : ITextDocumentRegistrationOptions, ISignatureHelpOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// List of characters that trigger signature help automatically.
    /// </summary>
    [JsonPropertyName("triggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? TriggerCharacters { get; set; }

    /// <summary>
    /// List of characters that re-trigger signature help.
    /// 
    /// These trigger characters are only active when signature help is already showing. All trigger characters
    /// are also counted as re-trigger characters.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("retriggerCharacters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? RetriggerCharacters { get; set; }
}

/// <summary>
/// Parameters for a {@link DefinitionRequest}.
/// </summary>
public sealed class DefinitionParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

/// <summary>
/// Registration options for a {@link DefinitionRequest}.
/// </summary>
public sealed class DefinitionRegistrationOptions : ITextDocumentRegistrationOptions, IDefinitionOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// Parameters for a {@link ReferencesRequest}.
/// </summary>
public sealed class ReferenceParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ReferenceContext Context { get; set; }
}

/// <summary>
/// Value-object that contains additional information when
/// requesting references.
/// </summary>
public sealed class ReferenceContext
{
    /// <summary>
    /// Include the declaration of the current symbol.
    /// </summary>
    [JsonPropertyName("includeDeclaration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean IncludeDeclaration { get; set; }
}

/// <summary>
/// Registration options for a {@link ReferencesRequest}.
/// </summary>
public sealed class ReferenceRegistrationOptions : ITextDocumentRegistrationOptions, IReferenceOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// Parameters for a {@link DocumentHighlightRequest}.
/// </summary>
public sealed class DocumentHighlightParams : ITextDocumentPositionParams, IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }
}

/// <summary>
/// A document highlight is a range inside a text document which deserves
/// special attention. Usually a document highlight is visualized by changing
/// the background color of its range.
/// </summary>
public sealed class DocumentHighlight
{
    /// <summary>
    /// The range this highlight applies to.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The highlight kind, default is {@link DocumentHighlightKind.Text text}.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DocumentHighlightKind? Kind { get; set; }
}

/// <summary>
/// A document highlight kind.
/// </summary>
public enum DocumentHighlightKind
{
    /// <summary>
    /// A textual occurrence.
    /// </summary>
    Text = 1,
    /// <summary>
    /// Read-access of a symbol, like reading a variable.
    /// </summary>
    Read = 2,
    /// <summary>
    /// Write-access of a symbol, like writing to a variable.
    /// </summary>
    Write = 3,
}

/// <summary>
/// Registration options for a {@link DocumentHighlightRequest}.
/// </summary>
public sealed class DocumentHighlightRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentHighlightOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// Parameters for a {@link DocumentSymbolRequest}.
/// </summary>
public sealed class DocumentSymbolParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// Represents information about programming constructs like variables, classes,
/// interfaces etc.
/// </summary>
public sealed class SymbolInformation : IBaseSymbolInformation
{
    /// <summary>
    /// The name of this symbol.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// The kind of this symbol.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this symbol.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// The name of the symbol containing this symbol. This information is for
    /// user interface purposes (e.g. to render a qualifier in the user interface
    /// if necessary). It can't be used to re-infer a hierarchy for the document
    /// symbols.
    /// </summary>
    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ContainerName { get; set; }

    /// <summary>
    /// Indicates if this symbol is deprecated.
    /// 
    /// @deprecated Use tags instead
    /// </summary>
    [Obsolete("Use tags instead")]
    [JsonPropertyName("deprecated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Deprecated { get; set; }

    /// <summary>
    /// The location of this symbol. The location's range is used by a tool
    /// to reveal the location in the editor. If the symbol is selected in the
    /// tool the range's start information is used to position the cursor. So
    /// the range usually spans more than the actual symbol's name and does
    /// normally include things like visibility modifiers.
    /// 
    /// The range doesn't have to denote a node range in the sense of an abstract
    /// syntax tree. It can therefore not be used to re-construct a hierarchy of
    /// the symbols.
    /// </summary>
    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Location Location { get; set; }
}

/// <summary>
/// A base for all symbol information.
/// </summary>
public sealed class BaseSymbolInformation : IBaseSymbolInformation
{
    /// <summary>
    /// The name of this symbol.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// The kind of this symbol.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this symbol.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// The name of the symbol containing this symbol. This information is for
    /// user interface purposes (e.g. to render a qualifier in the user interface
    /// if necessary). It can't be used to re-infer a hierarchy for the document
    /// symbols.
    /// </summary>
    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ContainerName { get; set; }
}

/// <summary>
/// A base for all symbol information.
/// </summary>
public interface IBaseSymbolInformation
{
    /// <summary>
    /// The name of this symbol.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Name { get; }

    /// <summary>
    /// The kind of this symbol.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public SymbolKind Kind { get; }

    /// <summary>
    /// Tags for this symbol.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; }

    /// <summary>
    /// The name of the symbol containing this symbol. This information is for
    /// user interface purposes (e.g. to render a qualifier in the user interface
    /// if necessary). It can't be used to re-infer a hierarchy for the document
    /// symbols.
    /// </summary>
    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ContainerName { get; }
}

/// <summary>
/// Represents programming constructs like variables, classes, interfaces etc.
/// that appear in a document. Document symbols can be hierarchical and they
/// have two ranges: one that encloses its definition and one that points to
/// its most interesting range, e.g. the range of an identifier.
/// </summary>
public sealed class DocumentSymbol
{
    /// <summary>
    /// The name of this symbol. Will be displayed in the user interface and therefore must not be
    /// an empty string or a string only consisting of white spaces.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// More detail for this symbol, e.g the signature of a function.
    /// </summary>
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Detail { get; set; }

    /// <summary>
    /// The kind of this symbol.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this document symbol.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// Indicates if this symbol is deprecated.
    /// 
    /// @deprecated Use tags instead
    /// </summary>
    [Obsolete("Use tags instead")]
    [JsonPropertyName("deprecated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Deprecated { get; set; }

    /// <summary>
    /// The range enclosing this symbol not including leading/trailing whitespace but everything else
    /// like comments. This information is typically used to determine if the clients cursor is
    /// inside the symbol to reveal in the symbol in the UI.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The range that should be selected and revealed when this symbol is being picked, e.g the name of a function.
    /// Must be contained by the `range`.
    /// </summary>
    [JsonPropertyName("selectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range SelectionRange { get; set; }

    /// <summary>
    /// Children of this symbol, e.g. properties of a class.
    /// </summary>
    [JsonPropertyName("children")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<DocumentSymbol>? Children { get; set; }
}

/// <summary>
/// Registration options for a {@link DocumentSymbolRequest}.
/// </summary>
public sealed class DocumentSymbolRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentSymbolOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// A human-readable string that is shown when multiple outlines trees
    /// are shown for the same document.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Label { get; set; }
}

/// <summary>
/// The parameters of a {@link CodeActionRequest}.
/// </summary>
public sealed class CodeActionParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The document in which the command was invoked.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The range for which the command was invoked.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// Context carrying additional information.
    /// </summary>
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required CodeActionContext Context { get; set; }
}

/// <summary>
/// Contains additional diagnostic information about the context in which
/// a {@link CodeActionProvider.provideCodeActions code action} is run.
/// </summary>
public sealed class CodeActionContext
{
    /// <summary>
    /// An array of diagnostics known on the client side overlapping the range provided to the
    /// `textDocument/codeAction` request. They are provided so that the server knows which
    /// errors are currently presented to the user for the given range. There is no guarantee
    /// that these accurately reflect the error state of the resource. The primary parameter
    /// to compute code actions is the provided range.
    /// </summary>
    [JsonPropertyName("diagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Diagnostic> Diagnostics { get; set; }

    /// <summary>
    /// Requested kind of actions to return.
    /// 
    /// Actions not of this kind are filtered out by the client before being shown. So servers
    /// can omit computing them.
    /// </summary>
    [JsonPropertyName("only")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<CodeActionKind>? Only { get; set; }

    /// <summary>
    /// The reason why code actions were requested.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("triggerKind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeActionTriggerKind? TriggerKind { get; set; }
}

/// <summary>
/// The reason why code actions were requested.
/// 
/// @since 3.17.0
/// </summary>
public enum CodeActionTriggerKind
{
    /// <summary>
    /// Code actions were explicitly requested by the user or by an extension.
    /// </summary>
    Invoked = 1,
    /// <summary>
    /// Code actions were requested automatically.
    /// 
    /// This typically happens when current selection in a file changes, but can
    /// also be triggered when file content changes.
    /// </summary>
    Automatic = 2,
}

/// <summary>
/// A code action represents a change that can be performed in code, e.g. to fix a problem or
/// to refactor code.
/// 
/// A CodeAction must set either `edit` and/or a `command`. If both are supplied, the `edit` is applied first, then the `command` is executed.
/// </summary>
public sealed class CodeAction
{
    public sealed class DisabledAction
    {
        /// <summary>
        /// Human readable description of why the code action is currently disabled.
        /// 
        /// This is displayed in the code actions UI.
        /// </summary>
        [JsonPropertyName("reason")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required System.String Reason { get; set; }
    }

    /// <summary>
    /// A short, human-readable, title for this code action.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Title { get; set; }

    /// <summary>
    /// The kind of the code action.
    /// 
    /// Used to filter code actions.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeActionKind? Kind { get; set; }

    /// <summary>
    /// The diagnostics that this code action resolves.
    /// </summary>
    [JsonPropertyName("diagnostics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<Diagnostic>? Diagnostics { get; set; }

    /// <summary>
    /// Marks this as a preferred action. Preferred actions are used by the `auto fix` command and can be targeted
    /// by keybindings.
    /// 
    /// A quick fix should be marked preferred if it properly addresses the underlying error.
    /// A refactoring should be marked preferred if it is the most reasonable choice of actions to take.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("isPreferred")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? IsPreferred { get; set; }

    /// <summary>
    /// Marks that the code action cannot currently be applied.
    /// 
    /// Clients should follow the following guidelines regarding disabled code actions:
    /// 
    ///   - Disabled code actions are not shown in automatic [lightbulbs](https://code.visualstudio.com/docs/editor/editingevolved#_code-action)
    ///     code action menus.
    /// 
    ///   - Disabled actions are shown as faded out in the code action menu when the user requests a more specific type
    ///     of code action, such as refactorings.
    /// 
    ///   - If the user has a [keybinding](https://code.visualstudio.com/docs/editor/refactoring#_keybindings-for-code-actions)
    ///     that auto applies a code action and only disabled code actions are returned, the client should show the user an
    ///     error message with `reason` in the editor.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("disabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CodeAction.DisabledAction? Disabled { get; set; }

    /// <summary>
    /// The workspace edit this code action performs.
    /// </summary>
    [JsonPropertyName("edit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public WorkspaceEdit? Edit { get; set; }

    /// <summary>
    /// A command this code action executes. If a code action
    /// provides an edit and a command, first the edit is
    /// executed and then the command.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Command? Command { get; set; }

    /// <summary>
    /// A data entry field that is preserved on a code action between
    /// a `textDocument/codeAction` and a `codeAction/resolve` request.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Registration options for a {@link CodeActionRequest}.
/// </summary>
public sealed class CodeActionRegistrationOptions : ITextDocumentRegistrationOptions, ICodeActionOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// CodeActionKinds that this server may return.
    /// 
    /// The list of kinds may be generic, such as `CodeActionKind.Refactor`, or the server
    /// may list out every specific kind they provide.
    /// </summary>
    [JsonPropertyName("codeActionKinds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<CodeActionKind>? CodeActionKinds { get; set; }

    /// <summary>
    /// The server provides support to resolve additional
    /// information for a code action.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// The parameters of a {@link WorkspaceSymbolRequest}.
/// </summary>
public sealed class WorkspaceSymbolParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// A query string to filter symbols by. Clients may send an empty
    /// string here to request all symbols.
    /// </summary>
    [JsonPropertyName("query")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Query { get; set; }
}

/// <summary>
/// A special workspace symbol that supports locations without a range.
/// 
/// See also SymbolInformation.
/// 
/// @since 3.17.0
/// </summary>
public sealed class WorkspaceSymbol : IBaseSymbolInformation
{
    public sealed class LocationSymbol
    {
        [JsonPropertyName("uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonRequired]
        public required Draco.Lsp.Model.DocumentUri Uri { get; set; }
    }

    /// <summary>
    /// The name of this symbol.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Name { get; set; }

    /// <summary>
    /// The kind of this symbol.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required SymbolKind Kind { get; set; }

    /// <summary>
    /// Tags for this symbol.
    /// 
    /// @since 3.16.0
    /// </summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<SymbolTag>? Tags { get; set; }

    /// <summary>
    /// The name of the symbol containing this symbol. This information is for
    /// user interface purposes (e.g. to render a qualifier in the user interface
    /// if necessary). It can't be used to re-infer a hierarchy for the document
    /// symbols.
    /// </summary>
    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ContainerName { get; set; }

    /// <summary>
    /// The location of the symbol. Whether a server is allowed to
    /// return a location without a range depends on the client
    /// capability `workspace.symbol.resolveSupport`.
    /// 
    /// See SymbolInformation#location for more details.
    /// </summary>
    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<Location, WorkspaceSymbol.LocationSymbol> Location { get; set; }

    /// <summary>
    /// A data entry field that is preserved on a workspace symbol between a
    /// workspace symbol request and a workspace symbol resolve request.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Registration options for a {@link WorkspaceSymbolRequest}.
/// </summary>
public sealed class WorkspaceSymbolRegistrationOptions : IWorkspaceSymbolOptions
{
    /// <summary>
    /// The server provides support to resolve additional
    /// information for a workspace symbol.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// The parameters of a {@link CodeLensRequest}.
/// </summary>
public sealed class CodeLensParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The document to request code lens for.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// A code lens represents a {@link Command command} that should be shown along with
/// source text, like the number of references, a way to run tests, etc.
/// 
/// A code lens is _unresolved_ when no command is associated to it. For performance
/// reasons the creation of a code lens and resolving should be done in two stages.
/// </summary>
public sealed class CodeLens
{
    /// <summary>
    /// The range in which this code lens is valid. Should only span a single line.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The command this code lens represents.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Command? Command { get; set; }

    /// <summary>
    /// A data entry field that is preserved on a code lens item between
    /// a {@link CodeLensRequest} and a [CodeLensResolveRequest]
    /// (#CodeLensResolveRequest)
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Registration options for a {@link CodeLensRequest}.
/// </summary>
public sealed class CodeLensRegistrationOptions : ITextDocumentRegistrationOptions, ICodeLensOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// Code lens has a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// The parameters of a {@link DocumentLinkRequest}.
/// </summary>
public sealed class DocumentLinkParams : IWorkDoneProgressParams, IPartialResultParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// An optional token that a server can use to report partial results (e.g. streaming) to
    /// the client.
    /// </summary>
    [JsonPropertyName("partialResultToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? PartialResultToken { get; set; }

    /// <summary>
    /// The document to provide document links for.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }
}

/// <summary>
/// A document link is a range in a text document that links to an internal or external resource, like another
/// text document or a web site.
/// </summary>
public sealed class DocumentLink
{
    /// <summary>
    /// The range this link applies to.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The uri this link points to. If missing a resolve request is sent later.
    /// </summary>
    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Uri? Target { get; set; }

    /// <summary>
    /// The tooltip text when you hover over this link.
    /// 
    /// If a tooltip is provided, is will be displayed in a string that includes instructions on how to
    /// trigger the link, such as `{0} (ctrl + click)`. The specific instructions vary depending on OS,
    /// user settings, and localization.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("tooltip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Tooltip { get; set; }

    /// <summary>
    /// A data entry field that is preserved on a document link between a
    /// DocumentLinkRequest and a DocumentLinkResolveRequest.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Text.Json.JsonElement? Data { get; set; }
}

/// <summary>
/// Registration options for a {@link DocumentLinkRequest}.
/// </summary>
public sealed class DocumentLinkRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentLinkOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// Document links have a resolve provider as well.
    /// </summary>
    [JsonPropertyName("resolveProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? ResolveProvider { get; set; }
}

/// <summary>
/// The parameters of a {@link DocumentFormattingRequest}.
/// </summary>
public sealed class DocumentFormattingParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The document to format.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The format options.
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required FormattingOptions Options { get; set; }
}

/// <summary>
/// Value-object describing what options formatting should use.
/// </summary>
public sealed class FormattingOptions
{
    /// <summary>
    /// Size of a tab in spaces.
    /// </summary>
    [JsonPropertyName("tabSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.UInt32 TabSize { get; set; }

    /// <summary>
    /// Prefer spaces over tabs.
    /// </summary>
    [JsonPropertyName("insertSpaces")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean InsertSpaces { get; set; }

    /// <summary>
    /// Trim trailing whitespace on a line.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("trimTrailingWhitespace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? TrimTrailingWhitespace { get; set; }

    /// <summary>
    /// Insert a newline character at the end of the file if one does not exist.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("insertFinalNewline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? InsertFinalNewline { get; set; }

    /// <summary>
    /// Trim all newlines after the final newline at the end of the file.
    /// 
    /// @since 3.15.0
    /// </summary>
    [JsonPropertyName("trimFinalNewlines")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? TrimFinalNewlines { get; set; }
}

/// <summary>
/// Registration options for a {@link DocumentFormattingRequest}.
/// </summary>
public sealed class DocumentFormattingRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentFormattingOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// The parameters of a {@link DocumentRangeFormattingRequest}.
/// </summary>
public sealed class DocumentRangeFormattingParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The document to format.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The range to format
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The format options
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required FormattingOptions Options { get; set; }
}

/// <summary>
/// Registration options for a {@link DocumentRangeFormattingRequest}.
/// </summary>
public sealed class DocumentRangeFormattingRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentRangeFormattingOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }
}

/// <summary>
/// The parameters of a {@link DocumentOnTypeFormattingRequest}.
/// </summary>
public sealed class DocumentOnTypeFormattingParams
{
    /// <summary>
    /// The document to format.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position around which the on type formatting should happen.
    /// This is not necessarily the exact position where the character denoted
    /// by the property `ch` got typed.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// The character that has been typed that triggered the formatting
    /// on type request. That is not necessarily the last character that
    /// got inserted into the document since the client could auto insert
    /// characters as well (e.g. like automatic brace completion).
    /// </summary>
    [JsonPropertyName("ch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Ch { get; set; }

    /// <summary>
    /// The formatting options.
    /// </summary>
    [JsonPropertyName("options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required FormattingOptions Options { get; set; }
}

/// <summary>
/// Registration options for a {@link DocumentOnTypeFormattingRequest}.
/// </summary>
public sealed class DocumentOnTypeFormattingRegistrationOptions : ITextDocumentRegistrationOptions, IDocumentOnTypeFormattingOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// A character on which formatting should be triggered, like `{`.
    /// </summary>
    [JsonPropertyName("firstTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String FirstTriggerCharacter { get; set; }

    /// <summary>
    /// More trigger characters.
    /// </summary>
    [JsonPropertyName("moreTriggerCharacter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.String>? MoreTriggerCharacter { get; set; }
}

/// <summary>
/// The parameters of a {@link RenameRequest}.
/// </summary>
public sealed class RenameParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The document to rename.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position at which this request was sent.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// The new name of the symbol. If the given name is not valid the
    /// request must return a {@link ResponseError} with an
    /// appropriate message set.
    /// </summary>
    [JsonPropertyName("newName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String NewName { get; set; }
}

/// <summary>
/// Registration options for a {@link RenameRequest}.
/// </summary>
public sealed class RenameRegistrationOptions : ITextDocumentRegistrationOptions, IRenameOptions
{
    /// <summary>
    /// A document selector to identify the scope of the registration. If set to null
    /// the document selector provided on the client side will be used.
    /// </summary>
    [JsonPropertyName("documentSelector")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<DocumentFilter>? DocumentSelector { get; set; }

    /// <summary>
    /// Renames should be checked and tested before being executed.
    /// 
    /// @since version 3.12.0
    /// </summary>
    [JsonPropertyName("prepareProvider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? PrepareProvider { get; set; }
}

public sealed class PrepareRenameParams : ITextDocumentPositionParams, IWorkDoneProgressParams
{
    /// <summary>
    /// The text document.
    /// </summary>
    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required ITextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// The position inside the text document.
    /// </summary>
    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Position Position { get; set; }

    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }
}

/// <summary>
/// The parameters of a {@link ExecuteCommandRequest}.
/// </summary>
public sealed class ExecuteCommandParams : IWorkDoneProgressParams
{
    /// <summary>
    /// An optional token that a server can use to report work done progress.
    /// </summary>
    [JsonPropertyName("workDoneToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OneOf<System.Int32, System.String>? WorkDoneToken { get; set; }

    /// <summary>
    /// The identifier of the actual command handler.
    /// </summary>
    [JsonPropertyName("command")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Command { get; set; }

    /// <summary>
    /// Arguments that the command should be invoked with.
    /// </summary>
    [JsonPropertyName("arguments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<System.Text.Json.JsonElement>? Arguments { get; set; }
}

/// <summary>
/// Registration options for a {@link ExecuteCommandRequest}.
/// </summary>
public sealed class ExecuteCommandRegistrationOptions : IExecuteCommandOptions
{
    /// <summary>
    /// The commands to be executed on the server
    /// </summary>
    [JsonPropertyName("commands")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<System.String> Commands { get; set; }
}

/// <summary>
/// The parameters passed via an apply workspace edit request.
/// </summary>
public sealed class ApplyWorkspaceEditParams
{
    /// <summary>
    /// An optional label of the workspace edit. This label is
    /// presented in the user interface for example on an undo
    /// stack to undo the workspace edit.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Label { get; set; }

    /// <summary>
    /// The edits to apply.
    /// </summary>
    [JsonPropertyName("edit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required WorkspaceEdit Edit { get; set; }
}

/// <summary>
/// The result returned from the apply workspace edit request.
/// 
/// @since 3.17 renamed from ApplyWorkspaceEditResponse
/// </summary>
public sealed class ApplyWorkspaceEditResult
{
    /// <summary>
    /// Indicates whether the edit was applied or not.
    /// </summary>
    [JsonPropertyName("applied")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean Applied { get; set; }

    /// <summary>
    /// An optional textual description for why the edit was not applied.
    /// This may be used by the server for diagnostic logging or to provide
    /// a suitable error for a request that triggered the edit.
    /// </summary>
    [JsonPropertyName("failureReason")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? FailureReason { get; set; }

    /// <summary>
    /// Depending on the client's failure handling strategy `failedChange` might
    /// contain the index of the change that failed. This property is only available
    /// if the client signals a `failureHandlingStrategy` in its client capabilities.
    /// </summary>
    [JsonPropertyName("failedChange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? FailedChange { get; set; }
}

public sealed class WorkDoneProgressBegin
{
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "begin";

    /// <summary>
    /// Mandatory title of the progress operation. Used to briefly inform about
    /// the kind of operation being performed.
    /// 
    /// Examples: "Indexing" or "Linking dependencies".
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Title { get; set; }

    /// <summary>
    /// Controls if a cancel button should show to allow the user to cancel the
    /// long running operation. Clients that don't support cancellation are allowed
    /// to ignore the setting.
    /// </summary>
    [JsonPropertyName("cancellable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Cancellable { get; set; }

    /// <summary>
    /// Optional, more detailed associated progress message. Contains
    /// complementary information to the `title`.
    /// 
    /// Examples: "3/25 files", "project/src/module2", "node_modules/some_dep".
    /// If unset, the previous progress message (if any) is still valid.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Message { get; set; }

    /// <summary>
    /// Optional progress percentage to display (value 100 is considered 100%).
    /// If not provided infinite progress is assumed and clients are allowed
    /// to ignore the `percentage` value in subsequent in report notifications.
    /// 
    /// The value should be steadily rising. Clients are free to ignore values
    /// that are not following this rule. The value range is [0, 100].
    /// </summary>
    [JsonPropertyName("percentage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? Percentage { get; set; }
}

public sealed class WorkDoneProgressReport
{
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "report";

    /// <summary>
    /// Controls enablement state of a cancel button.
    /// 
    /// Clients that don't support cancellation or don't support controlling the button's
    /// enablement state are allowed to ignore the property.
    /// </summary>
    [JsonPropertyName("cancellable")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? Cancellable { get; set; }

    /// <summary>
    /// Optional, more detailed associated progress message. Contains
    /// complementary information to the `title`.
    /// 
    /// Examples: "3/25 files", "project/src/module2", "node_modules/some_dep".
    /// If unset, the previous progress message (if any) is still valid.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Message { get; set; }

    /// <summary>
    /// Optional progress percentage to display (value 100 is considered 100%).
    /// If not provided infinite progress is assumed and clients are allowed
    /// to ignore the `percentage` value in subsequent in report notifications.
    /// 
    /// The value should be steadily rising. Clients are free to ignore values
    /// that are not following this rule. The value range is [0, 100]
    /// </summary>
    [JsonPropertyName("percentage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.UInt32? Percentage { get; set; }
}

public sealed class WorkDoneProgressEnd
{
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "end";

    /// <summary>
    /// Optional, a final message indicating to for example indicate the outcome
    /// of the operation.
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Message { get; set; }
}

public sealed class SetTraceParams
{
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required TraceValues Value { get; set; }
}

public sealed class LogTraceParams
{
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Message { get; set; }

    [JsonPropertyName("verbose")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Verbose { get; set; }
}

public sealed class CancelParams
{
    /// <summary>
    /// The request id to cancel.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.Int32, System.String> Id { get; set; }
}

public sealed class ProgressParams
{
    /// <summary>
    /// The progress token provided by the client or server.
    /// </summary>
    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required OneOf<System.Int32, System.String> Token { get; set; }

    /// <summary>
    /// The progress data.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Text.Json.JsonElement Value { get; set; }
}

/// <summary>
/// Represents the connection of two locations. Provides additional metadata over normal {@link Location locations},
/// including an origin range.
/// </summary>
public sealed class LocationLink
{
    /// <summary>
    /// Span of the origin of this link.
    /// 
    /// Used as the underlined span for mouse interaction. Defaults to the word range at
    /// the definition position.
    /// </summary>
    [JsonPropertyName("originSelectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Range? OriginSelectionRange { get; set; }

    /// <summary>
    /// The target resource identifier of this link.
    /// </summary>
    [JsonPropertyName("targetUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Draco.Lsp.Model.DocumentUri TargetUri { get; set; }

    /// <summary>
    /// The full target range of this link. If the target for example is a symbol then target range is the
    /// range enclosing this symbol not including leading/trailing whitespace but everything else
    /// like comments. This information is typically used to highlight the range in the editor.
    /// </summary>
    [JsonPropertyName("targetRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range TargetRange { get; set; }

    /// <summary>
    /// The range that should be selected and revealed when this link is being followed, e.g the name of a function.
    /// Must be contained by the `targetRange`. See also `DocumentSymbol#range`
    /// </summary>
    [JsonPropertyName("targetSelectionRange")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range TargetSelectionRange { get; set; }
}

/// <summary>
/// Provide inline value as text.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueText
{
    /// <summary>
    /// The document range for which the inline value applies.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// The text of the inline value.
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String Text { get; set; }
}

/// <summary>
/// Provide inline value through a variable lookup.
/// If only a range is specified, the variable name will be extracted from the underlying document.
/// An optional variable name can be used to override the extracted name.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueVariableLookup
{
    /// <summary>
    /// The document range for which the inline value applies.
    /// The range is used to extract the variable name from the underlying document.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// If specified the name of the variable to look up.
    /// </summary>
    [JsonPropertyName("variableName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? VariableName { get; set; }

    /// <summary>
    /// How to perform the lookup.
    /// </summary>
    [JsonPropertyName("caseSensitiveLookup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.Boolean CaseSensitiveLookup { get; set; }
}

/// <summary>
/// Provide an inline value through an expression evaluation.
/// If only a range is specified, the expression will be extracted from the underlying document.
/// An optional expression can be used to override the extracted expression.
/// 
/// @since 3.17.0
/// </summary>
public sealed class InlineValueEvaluatableExpression
{
    /// <summary>
    /// The document range for which the inline value applies.
    /// The range is used to extract the evaluatable expression from the underlying document.
    /// </summary>
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required Range Range { get; set; }

    /// <summary>
    /// If specified the expression overrides the extracted expression.
    /// </summary>
    [JsonPropertyName("expression")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Expression { get; set; }
}

/// <summary>
/// A full diagnostic report with a set of related documents.
/// 
/// @since 3.17.0
/// </summary>
public sealed class RelatedFullDocumentDiagnosticReport : IFullDocumentDiagnosticReport
{
    /// <summary>
    /// A full document diagnostic report.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "full";

    /// <summary>
    /// An optional result id. If provided it will
    /// be sent on the next diagnostic request for the
    /// same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? ResultId { get; set; }

    /// <summary>
    /// The actual items.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required IList<Diagnostic> Items { get; set; }

    /// <summary>
    /// Diagnostics of related documents. This information is useful
    /// in programming languages where code in a file A can generate
    /// diagnostics in a file B which A depends on. An example of
    /// such a language is C/C++ where marco definitions in a file
    /// a.cpp and result in errors in a header file b.hpp.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("relatedDocuments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<Draco.Lsp.Model.DocumentUri, OneOf<IFullDocumentDiagnosticReport, IUnchangedDocumentDiagnosticReport>>? RelatedDocuments { get; set; }
}

/// <summary>
/// An unchanged diagnostic report with a set of related documents.
/// 
/// @since 3.17.0
/// </summary>
public sealed class RelatedUnchangedDocumentDiagnosticReport : IUnchangedDocumentDiagnosticReport
{
    /// <summary>
    /// A document diagnostic report indicating
    /// no changes to the last result. A server can
    /// only return `unchanged` if result ids are
    /// provided.
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public System.String Kind => "unchanged";

    /// <summary>
    /// A result id which will be sent on the next
    /// diagnostic request for the same document.
    /// </summary>
    [JsonPropertyName("resultId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonRequired]
    public required System.String ResultId { get; set; }

    /// <summary>
    /// Diagnostics of related documents. This information is useful
    /// in programming languages where code in a file A can generate
    /// diagnostics in a file B which A depends on. An example of
    /// such a language is C/C++ where marco definitions in a file
    /// a.cpp and result in errors in a header file b.hpp.
    /// 
    /// @since 3.17.0
    /// </summary>
    [JsonPropertyName("relatedDocuments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<Draco.Lsp.Model.DocumentUri, OneOf<IFullDocumentDiagnosticReport, IUnchangedDocumentDiagnosticReport>>? RelatedDocuments { get; set; }
}

/// <summary>
/// A set of predefined token types. This set is not fixed
/// an clients can specify additional token types via the
/// corresponding client capabilities.
/// 
/// @since 3.16.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum SemanticTokenTypes
{
    [EnumMember(Value = "namespace")]
    Namespace,
    /// <summary>
    /// Represents a generic type. Acts as a fallback for types which can't be mapped to
    /// a specific type like class or enum.
    /// </summary>
    [EnumMember(Value = "type")]
    Type,
    [EnumMember(Value = "class")]
    Class,
    [EnumMember(Value = "enum")]
    Enum,
    [EnumMember(Value = "interface")]
    Interface,
    [EnumMember(Value = "struct")]
    Struct,
    [EnumMember(Value = "typeParameter")]
    TypeParameter,
    [EnumMember(Value = "parameter")]
    Parameter,
    [EnumMember(Value = "variable")]
    Variable,
    [EnumMember(Value = "property")]
    Property,
    [EnumMember(Value = "enumMember")]
    EnumMember,
    [EnumMember(Value = "event")]
    Event,
    [EnumMember(Value = "function")]
    Function,
    [EnumMember(Value = "method")]
    Method,
    [EnumMember(Value = "macro")]
    Macro,
    [EnumMember(Value = "keyword")]
    Keyword,
    [EnumMember(Value = "modifier")]
    Modifier,
    [EnumMember(Value = "comment")]
    Comment,
    [EnumMember(Value = "string")]
    String,
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "regexp")]
    Regexp,
    [EnumMember(Value = "operator")]
    Operator,
    /// <summary>
    /// @since 3.17.0
    /// </summary>
    [EnumMember(Value = "decorator")]
    Decorator,
}

/// <summary>
/// A set of predefined token modifiers. This set is not fixed
/// an clients can specify additional token types via the
/// corresponding client capabilities.
/// 
/// @since 3.16.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum SemanticTokenModifiers
{
    [EnumMember(Value = "declaration")]
    Declaration,
    [EnumMember(Value = "definition")]
    Definition,
    [EnumMember(Value = "readonly")]
    Readonly,
    [EnumMember(Value = "static")]
    Static,
    [EnumMember(Value = "deprecated")]
    Deprecated,
    [EnumMember(Value = "abstract")]
    Abstract,
    [EnumMember(Value = "async")]
    Async,
    [EnumMember(Value = "modification")]
    Modification,
    [EnumMember(Value = "documentation")]
    Documentation,
    [EnumMember(Value = "defaultLibrary")]
    DefaultLibrary,
}

/// <summary>
/// The document diagnostic report kinds.
/// 
/// @since 3.17.0
/// </summary>
[JsonConverter(typeof(EnumValueConverter))]
public enum DocumentDiagnosticReportKind
{
    /// <summary>
    /// A diagnostic report with a full
    /// set of problems.
    /// </summary>
    [EnumMember(Value = "full")]
    Full,
    /// <summary>
    /// A report indicating that the last
    /// returned report is still accurate.
    /// </summary>
    [EnumMember(Value = "unchanged")]
    Unchanged,
}

/// <summary>
/// Predefined error codes.
/// </summary>
public enum ErrorCodes
{
    ParseError = -32700,
    InvalidRequest = -32600,
    MethodNotFound = -32601,
    InvalidParams = -32602,
    InternalError = -32603,
    /// <summary>
    /// Error code indicating that a server received a notification or
    /// request before the server has received the `initialize` request.
    /// </summary>
    ServerNotInitialized = -32002,
    UnknownErrorCode = -32001,
}

public enum LSPErrorCodes
{
    /// <summary>
    /// A request failed but it was syntactically correct, e.g the
    /// method name was known and the parameters were valid. The error
    /// message should contain human readable information about why
    /// the request failed.
    /// 
    /// @since 3.17.0
    /// </summary>
    RequestFailed = -32803,
    /// <summary>
    /// The server cancelled the request. This error code should
    /// only be used for requests that explicitly support being
    /// server cancellable.
    /// 
    /// @since 3.17.0
    /// </summary>
    ServerCancelled = -32802,
    /// <summary>
    /// The server detected that the content of a document got
    /// modified outside normal conditions. A server should
    /// NOT send this error code if it detects a content change
    /// in it unprocessed messages. The result even computed
    /// on an older state might still be useful for the client.
    /// 
    /// If a client decides that a result is not of any use anymore
    /// the client should cancel the request.
    /// </summary>
    ContentModified = -32801,
    /// <summary>
    /// The client has canceled a request and a server as detected
    /// the cancel.
    /// </summary>
    RequestCancelled = -32800,
}

public sealed class PrepareRenameResult
{
    [JsonPropertyName("range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Range? Range { get; set; }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Placeholder { get; set; }

    [JsonPropertyName("defaultBehavior")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.Boolean? DefaultBehavior { get; set; }
}

public sealed class TextDocumentFilter
{
    /// <summary>
    /// A language id, like `typescript`.
    /// </summary>
    [JsonPropertyName("language")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Language { get; set; }

    /// <summary>
    /// A Uri {@link Uri.scheme scheme}, like `file` or `untitled`.
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Scheme { get; set; }

    /// <summary>
    /// A glob pattern, like `*.{ts,js}`.
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Pattern { get; set; }
}

public sealed class NotebookDocumentFilter
{
    /// <summary>
    /// The type of the enclosing notebook.
    /// </summary>
    [JsonPropertyName("notebookType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? NotebookType { get; set; }

    /// <summary>
    /// A Uri {@link Uri.scheme scheme}, like `file` or `untitled`.
    /// </summary>
    [JsonPropertyName("scheme")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Scheme { get; set; }

    /// <summary>
    /// A glob pattern.
    /// </summary>
    [JsonPropertyName("pattern")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public System.String? Pattern { get; set; }
}
#pragma warning restore CS9042
