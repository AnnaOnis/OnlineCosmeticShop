import React, { createContext, useContext, useState, useEffect } from 'react';
import { AuthService } from './apiClient/http-services/auth.service';
import { CustomerRegisterRequestDto, LogoutRequest } from './apiClient/models';

interface AuthContextType {
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  register: (customer: CustomerRegisterRequestDto) => Promise<void>;
}

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  login: async () => {},
  logout: () => {},
  register: async () => {},
});

export const useAuth = () => useContext(AuthContext);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const authService = new AuthService('/api');

  useEffect(() => {
    // Проверяем наличие токена в localStorage при загрузке приложения
    const token = localStorage.getItem('jwtToken');
    if (token) {
      setIsAuthenticated(true);
    }
  }, []);

  const login = async (email: string, password: string) => {
    try {
      await authService.login({ email, password }, new AbortController().signal);
      if(localStorage.getItem('jwtToken')) setIsAuthenticated(true);    
    } catch (error) {
      console.error('Ошибка при входе:', error);
      alert('Неверный email или пароль');
    }
  };

  const logout = async () => {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const logoutRequest: LogoutRequest = {
        token: token
      };
      try {
        await authService.logout(logoutRequest, new AbortController().signal);
        setIsAuthenticated(false);
        window.location.href = '/login';
      } catch (error) {
        console.error('Ошибка при выходе:', error);
        alert('Ошибка при выходе. Попробуйте снова.');
      }
    } else {
      localStorage.removeItem('jwtToken');
      setIsAuthenticated(false);
      window.location.href = '/login';
    }
  };

  const register = async (customer: CustomerRegisterRequestDto) => {
    try {
      await authService.register(customer, new AbortController().signal);
      if(localStorage.getItem('jwtToken')) setIsAuthenticated(true);
      setIsAuthenticated(true);
    } catch (error) {
      console.error('Ошибка при регистрации:', error);
      alert('Ошибка при регистрации. Попробуйте снова.');
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout, register }}>
      {children}
    </AuthContext.Provider>
  );
};