# Enterprise Knowledge Assistant

An enterprise-style **Retrieval-Augmented Generation (RAG) backend API** built with **ASP.NET Core, .NET 8, SQL Server, and Ollama**.

The application allows PDF documents to be uploaded, extracts and chunks their text, generates vector embeddings, stores them in SQL Server, and uses semantic search to retrieve relevant document content before generating a grounded answer with a local Large Language Model (LLM).

The project is designed using **Clean Architecture** principles with clear separation between API, Application, Domain, Infrastructure, and Persistence layers.

---

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Ollama
- `nomic-embed-text` — embedding model
- `llama3.2:3b` — local LLM
- PdfPig — PDF text extraction
- REST APIs
- Dependency Injection
- Clean Architecture
- Retrieval-Augmented Generation (RAG)
- Vector embeddings
- Cosine similarity

---

## Architecture

The solution is divided into the following projects:

```text
EnterpriseKnowledgeAssistant
│
├── EnterpriseKnowledgeAssistant.API
├── EnterpriseKnowledgeAssistant.Application
├── EnterpriseKnowledgeAssistant.Domain
├── EnterpriseKnowledgeAssistant.Infrastructure
└── EnterpriseKnowledgeAssistant.Persistence
```

### Layer Responsibilities

**API**

Exposes REST endpoints for document upload, semantic search, and RAG-based question answering.

**Application**

Contains application services, interfaces, DTOs, and RAG orchestration logic.

**Domain**

Contains the core domain entities and enums, including `KnowledgeDocument`, `DocumentChunk`, and `DocumentStatus`.

**Infrastructure**

Contains implementations for PDF extraction, text chunking, embedding generation, semantic search, cosine similarity, and LLM communication.

**Persistence**

Contains the EF Core `DbContext`, repository implementations, entity configuration, and SQL Server integration.

---

## RAG Workflow

The application implements the three major stages of a RAG system:

### 1. Indexing

```text
PDF Document
      ↓
Text Extraction
      ↓
Sentence-Aware Chunking
      ↓
Embedding Generation
(nomic-embed-text)
      ↓
Document Chunks + Embeddings
      ↓
SQL Server
```

When a PDF is uploaded:

1. Document metadata is stored.
2. Text is extracted using PdfPig.
3. Extracted text is divided into sentence-aware chunks.
4. Each chunk is converted into a vector embedding using `nomic-embed-text`.
5. The chunk and its embedding are persisted in SQL Server.

---

### 2. Retrieval

```text
User Question
      ↓
Question Embedding
      ↓
Cosine Similarity
      ↓
Compare Against Stored
Chunk Embeddings
      ↓
Top-K Relevant Chunks
```

The user's question is converted into an embedding using the same embedding model used during document ingestion.

Cosine similarity is calculated between the question embedding and stored document embeddings.

The chunks with the highest similarity scores are selected as the context for the LLM.

---

### 3. Augmentation and Generation

```text
Question
   +
Retrieved Top-K Chunks
   +
System Instructions
        ↓
   Augmented Prompt
        ↓
     llama3.2:3b
        ↓
   Grounded Answer
```

The `RagService` constructs a prompt containing:

- The user's question
- Retrieved document chunks
- Grounding instructions

The prompt instructs the model to answer only from the provided document context.

The augmented prompt is then sent to the locally running `llama3.2:3b` model through Ollama.

---

## Architecture Diagram

![Architecture Diagram](docs/architecture.png)

---

## UML Class Diagram

![UML Class Diagram](docs/uml-diagram.png)

---

## Core Components

### DocumentService

Coordinates the document ingestion process.

Responsibilities include:

- Store document metadata
- Extract PDF text
- Split text into chunks
- Generate embeddings
- Store document chunks and embeddings
- Track document processing status

### PdfTextExtractor

Uses **PdfPig** to extract text from uploaded PDF documents.

### TextChunker

Implements sentence-aware chunking to reduce the chance of breaking meaningful sentences across chunk boundaries.

The current configuration uses approximately:

```text
Target chunk size: 1500 characters
Overlap:           200 characters
```

### OllamaEmbeddingService

Communicates with the local Ollama API and generates vector embeddings using:

```text
nomic-embed-text
```

The same embedding model is used for both document chunks and user questions.

### SemanticSearchService

Performs semantic retrieval by:

1. Generating the question embedding
2. Loading stored document chunk embeddings
3. Calculating cosine similarity
4. Ranking chunks by similarity
5. Returning the Top-K results

### RagService

Orchestrates the complete RAG process:

```text
Retrieval
   ↓
Augmentation
   ↓
Generation
```

The service retrieves relevant chunks, constructs the grounded prompt, sends it to the LLM, and returns the generated answer together with source information.

### OllamaLlmService

Communicates with Ollama's generation API using:

```text
llama3.2:3b
```

The LLM receives the augmented prompt and generates the final answer.

---

## Data Model

### KnowledgeDocument

Represents an uploaded document.

Important fields include:

```text
Id
FileName
ContentType
FileSize
Status
CreatedOn
ProcessedOn
```

### DocumentChunk

Represents a searchable portion of an uploaded document.

Important fields include:

```text
Id
DocumentId
ChunkIndex
Content
CharacterCount
Embedding
CreatedOn
```

Each `KnowledgeDocument` can contain multiple `DocumentChunk` records.

---

## Document Processing Status

Documents move through the following states:

```text
Uploaded
   ↓
Processing
   ↓
Processed
```

If processing fails:

```text
Processing
   ↓
Failed
```

---

## API Endpoints

### Upload Document

```http
POST /api/documents
Content-Type: multipart/form-data
```

Uploads a PDF document and starts the document processing pipeline.

---

### Semantic Search

```http
GET /api/search?query={query}&topK=5
```

Returns the document chunks most semantically related to the supplied query.

Example:

```text
GET /api/search?query=activity tracker duration&topK=5
```

---

### Ask the Knowledge Assistant

```http
GET /api/rag/ask?question={question}&topK=5
```

Executes the complete RAG pipeline.

Example:

```text
GET /api/rag/ask?question=what is the duration of activity tracker?&topK=5
```

Example response:

```json
{
  "question": "what is the duration of activity tracker?",
  "answer": "The duration of the Activity Tracker project is 9 months.",
  "sources": [
    {
      "documentId": "...",
      "chunkIndex": 8,
      "similarity": 0.6078
    }
  ]
}
```

---

## Running the Project Locally

### Prerequisites

Install:

- .NET 8 SDK
- SQL Server
- Ollama

Verify Ollama is running locally:

```text
http://localhost:11434
```

### Download the Models

Pull the embedding model:

```bash
ollama pull nomic-embed-text
```

Pull the LLM:

```bash
ollama pull llama3.2:3b
```

### Database

Configure the SQL Server connection string in the application's configuration.

Do not commit production credentials or secrets to the repository.

Apply the EF Core migrations to create the database.

The application currently uses:

```text
KnowledgeDocuments
DocumentChunks
```

### Start the API

Run:

```bash
dotnet run --project EnterpriseKnowledgeAssistant.API
```

Then use Swagger or an HTTP client to upload a PDF and query the knowledge assistant.

---

## Why RAG?

A normal LLM does not automatically know the contents of private enterprise documents.

RAG solves this by retrieving relevant information before asking the LLM to generate an answer.

```text
Without RAG

Question → LLM → Answer


With RAG

Question
   ↓
Semantic Retrieval
   ↓
Relevant Enterprise Data
   ↓
Augmented Prompt
   ↓
LLM
   ↓
Grounded Answer
```

This allows the model to answer questions using application-specific knowledge without retraining the LLM.

---

## Current Implementation Notes

The current version intentionally keeps vector search simple for learning and demonstration purposes.

Embeddings are serialized and stored in SQL Server, and cosine similarity is calculated by the application when performing semantic search.

This makes the complete RAG workflow easy to understand and debug before introducing specialized vector-search infrastructure.

---

## Planned Improvements

Future improvements may include:

- Similarity threshold / retrieval confidence filtering
- Batch embedding generation
- Token-aware or semantic chunking
- Dedicated vector storage and vector indexing
- Improved source citations
- Conversation history
- Asynchronous document processing
- Background workers
- Retry and resilience policies
- Prompt-injection protection
- Authentication and authorization
- Logging and observability
- RAG evaluation and retrieval metrics
- Docker support
- Automated tests

---

## Key Learning Areas

This project demonstrates practical implementation of:

- Clean Architecture in ASP.NET Core
- Dependency inversion
- Repository pattern
- PDF document processing
- Text chunking
- Vector embeddings
- Semantic similarity
- Cosine similarity
- Retrieval-Augmented Generation
- Prompt augmentation
- Local LLM integration
- Grounded AI responses
- SQL Server persistence
- REST API design

---

## Project Status

The core RAG pipeline is operational:

```text
PDF Upload
   ✓
Text Extraction
   ✓
Sentence-Aware Chunking
   ✓
Embedding Generation
   ✓
SQL Server Persistence
   ✓
Question Embedding
   ✓
Semantic Search
   ✓
Top-K Retrieval
   ✓
Prompt Augmentation
   ✓
LLM Generation
   ✓
Grounded Answer
   ✓
```

The next stages focus on improving retrieval quality, scalability, security, observability, and production readiness.
