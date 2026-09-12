import { useState } from 'react';
import { useAuth } from '../auth-context';
import { login } from '../api';

type LoginFormProps = {
  onSuccess?: () => void;
};

export function LoginForm({ onSuccess }: LoginFormProps) {
  const { login: setAuth } = useAuth();
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('123456');
  const [error, setError] = useState('');

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError('');

    try {
      const data = await login(username, password);
      const nextRole = username === 'admin' ? 'Admin' : 'User';
      setAuth(data.token, nextRole);
      onSuccess?.();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error desconocido');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="product-form">
      <label>
        Usuario
        <input value={username} onChange={(e) => setUsername(e.target.value)} />
      </label>

      <label>
        Contraseña
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      </label>

      <button type="submit">Iniciar sesión</button>
      {error && <p className="error-message">{error}</p>}
    </form>
  );
}
