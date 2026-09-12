import { useState } from 'react';
import { createProducto } from './api';
import { useAuth } from './auth-context';
import { LoginForm } from './components/LoginForm';
import { ProductForm } from './components/ProductForm';
import { ProductList } from './components/ProductList';
import { useProductos } from './hooks/useProductos';

type Producto = {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
};

export default function App() {
  const { token, role, isAuthenticated, isAdmin, logout } = useAuth();
  const { productos, reload } = useProductos();
  const [nombre, setNombre] = useState('');
  const [precio, setPrecio] = useState('');
  const [stock, setStock] = useState('');
  const [error, setError] = useState('');

  const handleLogout = () => {
    logout();
    setError('');
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError('');

    if (!token) {
      setError('Debes iniciar sesión antes de crear un producto.');
      return;
    }

    try {
      await createProducto(token, {
        nombre,
        precio: Number(precio),
        stock: Number(stock)
      });

      setNombre('');
      setPrecio('');
      setStock('');
      await reload();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error desconocido');
    }
  };

  return (
    <div className="app-shell">
      <h1>TalentMatch - Productos</h1>

      {!isAuthenticated ? (
        <LoginForm onSuccess={() => reload()} />
      ) : (
        <>
          <div className="token-box">
            <strong>Token activo</strong>
            <span>{token.substring(0, 20)}...</span>
            <small>Rol actual: {role}</small>
            <button type="button" className="logout-button" onClick={handleLogout}>Cerrar sesión</button>
          </div>

          <div className={`role-panel ${isAdmin ? 'admin-panel' : 'user-panel'}`}>
            {isAdmin ? (
              <>
                <h2>Panel de administración</h2>
                <ProductForm
                  nombre={nombre}
                  precio={precio}
                  stock={stock}
                  onNombreChange={setNombre}
                  onPrecioChange={setPrecio}
                  onStockChange={setStock}
                  onSubmit={handleSubmit}
                />
              </>
            ) : (
              <>
                <h2>Panel de consulta</h2>
                <div className="info-message">
                  Usuario con permisos de consulta. No puedes crear productos.
                </div>
              </>
            )}
          </div>
        </>
      )}

      {error && <p className="error-message">{error}</p>}

      {isAuthenticated && <ProductList productos={productos} />}
    </div>
  );
}
