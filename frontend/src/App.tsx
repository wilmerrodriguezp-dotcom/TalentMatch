import { useAuth } from './auth-context';
import { LoginForm } from './components/LoginForm';

export default function App() {
  const { token, role, isAuthenticated, isAdmin, logout } = useAuth();

  return (
    <div className="app-shell">
      <h1>TalentMatch</h1>

      {!isAuthenticated ? (
        <LoginForm />
      ) : (
        <>
          <div className="token-box">
            <strong>Token activo</strong>
            <span>{token.substring(0, 20)}...</span>
            <small>Rol actual: {role}</small>
            <button type="button" className="logout-button" onClick={logout}>Cerrar sesión</button>
          </div>

          <div className={`role-panel ${isAdmin ? 'admin-panel' : 'user-panel'}`}>
            <h2>Sesión activa</h2>
            <p>{isAdmin ? 'Autenticado como administrador.' : 'Autenticado como usuario.'}</p>
          </div>
        </>
      )}
    </div>
  );
}
