import '../styles/Header.css';
import { useAuth } from '../context/AuthContext';
import RoleBasedHeader from './AdminHeader';
import ClientHeader from './ClientHeader';


const Header = () => {
  const { isAuthenticated, role } = useAuth();

  if (isAuthenticated && role !== null) {
    return <RoleBasedHeader />;
  }

  return <ClientHeader />;
};

export default Header;