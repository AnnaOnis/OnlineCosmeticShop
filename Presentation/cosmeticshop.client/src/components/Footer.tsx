import AdminFooter from './AdminFooter';
import ClientFooter from './ClientFooter';
import { useAuth } from '../context/AuthContext';

const Footer = () => {
  const { isAuthenticated, role } = useAuth();

  if (isAuthenticated && role !== null) {
    return <AdminFooter />;
  }

  return <ClientFooter />;
};

export default Footer;