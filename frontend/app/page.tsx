"use client";

import { useState } from "react";
import axios from "axios";

// Define the Document Interface
interface DocumentResult {
  id: string;
  path: string;
  content: string;
}

export default function Home() {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState<DocumentResult[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!query) return;

    setLoading(true);
    setError("");
    setResults([]);

    try {
      const response = await axios.get<DocumentResult[]>(
        `http://localhost:5123/api/documents/search?query=${query}`
      );
      setResults(response.data);
    } catch (err: any) {
      setError(`Error: ${err.message || "Unknown error"}. Check Console for details.`);
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="min-h-screen bg-neutral-900 text-neutral-100 p-8">
      <div className="max-w-4xl mx-auto space-y-8">
        
        {/* Header */}
        <div className="text-center space-y-4">
          <h1 className="text-5xl font-bold bg-gradient-to-r from-blue-400 to-purple-500 bg-clip-text text-transparent">
            Vault Search
          </h1>
          <p className="text-neutral-400">Search your indexed documents instantly.</p>
        </div>

        {/* Search Bar */}
        <form onSubmit={handleSearch} className="flex gap-4">
          <input
            type="text"
            className="flex-1 bg-neutral-800 border border-neutral-700 rounded-lg px-4 py-3 focus:ring-2 focus:ring-blue-500 outline-none text-lg transition-all"
            placeholder="Search for keywords (e.g., 'pdf', 'invoice')..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button
            type="submit"
            disabled={loading}
            className="bg-blue-600 hover:bg-blue-700 text-white font-medium px-8 py-3 rounded-lg transition-colors disabled:opacity-50"
          >
            {loading ? "Searching..." : "Search"}
          </button>
        </form>

        {/* Error Message */}
        {error && (
          <div className="p-4 bg-red-900/50 border border-red-800 rounded-lg text-red-200">
            {error}
          </div>
        )}

        {/* Results Grid */}
        <div className="grid gap-6">
          {results.length > 0 ? (
            results.map((doc) => (
              <div
                key={doc.id}
                className="bg-neutral-800 border border-neutral-700 p-6 rounded-xl hover:border-neutral-500 transition-colors"
              >
                <div className="flex items-center justify-between mb-2">
                    <span className="text-xs font-mono text-neutral-500 truncate flex-1 min-w-0 mr-4" title={doc.path}>
                        {doc.path}
                    </span>
                    <span className="text-xs bg-neutral-700 px-2 py-1 rounded text-neutral-300 shrink-0">
                        Match
                    </span>
                </div>
                
                {/* Content Snippet (Truncated) */}
                <p className="text-neutral-300 leading-relaxed break-all">
                  {doc.content.substring(0, 300)}
                  {doc.content.length > 300 && "..."}
                </p>
              </div>
            ))
          ) : (
            !loading && results.length === 0 && query && (
                <div className="text-center text-neutral-500 py-10">No results found.</div>
            )
          )}
        </div>

      </div>
    </main>
  );
}
