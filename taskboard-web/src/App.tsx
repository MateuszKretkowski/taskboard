import { useEffect, useState } from "react";
import type { CurrentUserResponse } from "./api/types";
import { getCurrentUser, logout } from "./api/auth";
import { LoginForm } from "./components/LoginForm";
import { ProjectsPage } from "./pages/ProjectsPage";

export function App() {
  const [session, setSession] = useState<Session>({ kind: 'checking' });
  
  useEffect(() => {
    let ignore = false;
    getCurrentUser().then(user => { if (!ignore) { setSession(user === null ? { kind: 'anonymous' } : { kind: 'authenticated', user }) } })
      .catch(() => {
        if (!ignore) {
          setSession({ kind: 'anonymous' })
        }
      });
      
      return () => {
        ignore = true;
      }
    }, [])
    
    
  type Session = | { kind: 'checking' } | { kind: 'anonymous' } | { kind: 'authenticated'; user: CurrentUserResponse };

  async function handleLogout() {
    await logout();
    setSession({ kind: 'anonymous' });
  }

  return (
    <div className="mx-auto max-w-3xl px-4 pt-6 pb-12">
      <header className="mb-6 flex items-center justify-between">
        <h1>TaskBoard</h1>
        {session.kind === 'authenticated' && <span className="text-gray-500">{session.user.email}</span>}
        <button onClick={handleLogout}>Wyloguj sie</button>
      </header>

      {session.kind === 'checking' && <p>Sprawdzam sesje...</p>}
      {session.kind === 'anonymous' && 
        <LoginForm onLoggedIn={(user: CurrentUserResponse) => {setSession({ kind: 'authenticated', user: user })}} />
      }
      {session.kind === 'authenticated' && 
        <ProjectsPage />
      }
    </div>
  );
}
