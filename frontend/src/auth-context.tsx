import { createContext, useContext, useEffect, useMemo, useState } from 'react';

const STORAGE_KEY = 'talentmatch-token';
const ROLE_KEY = 'talentmatch-role';

type AuthContextType = {
  token: string;
  role: string;
  isAuthenticated: boolean;
  isAdmin: boolean;
  login: (newToken: string, newRole: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string>(() => localStorage.getItem(STORAGE_KEY) ?? '');
  const [role, setRole] = useState<string>(() => localStorage.getItem(ROLE_KEY) ?? '');

  useEffect(() => {
    if (token) {
      localStorage.setItem(STORAGE_KEY, token);
    } else {
      localStorage.removeItem(STORAGE_KEY);
    }
  }, [token]);

  useEffect(() => {
    if (role) {
      localStorage.setItem(ROLE_KEY, role);
    } else {
      localStorage.removeItem(ROLE_KEY);
    }
  }, [role]);

  const value = useMemo<AuthContextType>(() => ({
    token,
    role,
    isAuthenticated: Boolean(token),
    isAdmin: role === 'Admin',
    login: (newToken, newRole) => {
      setToken(newToken);
      setRole(newRole);
    },
    logout: () => {
      setToken('');
      setRole('');
    }
  }), [token, role]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error('useAuth debe usarse dentro de AuthProvider');
  }

  return context;
}
