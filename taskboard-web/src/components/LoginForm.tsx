import { useState, type SubmitEvent } from "react";
import { login, register } from "../api/auth";
import { ErrorMessage } from "./ErrorMessage";
import { readMessage } from "../api/client";
import type { CurrentUserResponse } from "../api/types";

type LoginFormProps = {
  onLoggedIn: (user: CurrentUserResponse) => void;
};

type mode = 'login' | 'register';

export function LoginForm({ onLoggedIn }: LoginFormProps) {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [currMode, setCurrMode] = useState<mode>('login');
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        setIsSubmitting(true);
        setError(null);
        try {
            const user = currMode === 'login' ? await login(email, password) : await register(email, password);
            onLoggedIn(user);
        }
        catch (error) {
            setError(readMessage(error));
        }
        finally {
            setIsSubmitting(false);
        }
    }

  return (
<form onSubmit={handleSubmit}>
      <h2>{currMode === 'login' ? 'Logowanie' : 'Nowe konto'}</h2>
      <label>
        E-mail
        <input type="email" value={email} onChange={event => setEmail(event.target.value)} autoComplete="email" required />
      </label>
      <label>
        Hasło
        <input
          type="password"
          value={password}
          onChange={event => setPassword(event.target.value)}
          autoComplete={currMode === 'login' ? 'current-password' : 'new-password'}
          minLength={currMode === 'register' ? 8 : undefined}
          required
        />
      </label>
      <ErrorMessage message={error} />
      <div className="flex flex-wrap items-center gap-1.5">
        <button type="submit" className="border-blue-700 bg-blue-700 text-white" disabled={isSubmitting}>
          {currMode === 'login' ? 'Zaloguj' : 'Załóż konto'}
        </button>
        <button type="button" onClick={() => setCurrMode(currMode === 'login' ? 'register' : 'login')}>
          {currMode === 'login' ? 'Nie mam konta' : 'Mam już konto'}
        </button>
      </div>
    </form>
  );
}
