import { useCallback, useEffect, useState } from 'react';
import { getProductos } from '../api';
import { useAuth } from '../auth-context';

type Producto = {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
};

export function useProductos() {
  const { token } = useAuth();
  const [productos, setProductos] = useState<Producto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const loadProductos = useCallback(async () => {
    if (!token) {
      setProductos([]);
      return;
    }

    setLoading(true);
    setError('');

    try {
      const data = await getProductos(token);
      setProductos(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error desconocido');
    } finally {
      setLoading(false);
    }
  }, [token]);

  useEffect(() => {
    loadProductos();
  }, [loadProductos]);

  return { productos, loading, error, reload: loadProductos };
}
