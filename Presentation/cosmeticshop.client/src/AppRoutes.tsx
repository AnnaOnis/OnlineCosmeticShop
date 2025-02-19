import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import CustomerProfile from './pages/CustomerProfile';
import ProductCatalog from './pages/ProductCatalog';
import ProductDetails from './pages/ProductDetails';
import Cart from './pages/Cart';
import Checkout from './pages/Checkout';
import OrderConfirmation from './pages/OrderConfirmation';
import Search from './pages/Search';
import Contacts from './pages/Contacts';
import Help from './pages/Help';
import Header from './components/Header';
import Footer from './components/Footer';
import AboutPage from './pages/AboutPage';
import OrderDetails from './pages/OrderDetails';
import AdminProfile from './pages/admin/AdminProfile';
import ProductAdminTable from './pages/admin/ProductAdminTable';
import ProductEditForm from './pages/admin/ProductEditForm';
import ProductAddForm from './pages/admin/ProductAddForm';
import OrderAdminTable from './pages/admin/OrderAdminTable';
import ReviewAdminTable from './pages/admin/ReviewAdminTable';

// const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
//   const { isAuthenticated, role } = useAuth();

//   if (!isAuthenticated) {
//     return <Navigate to="/login" replace />;
//   } else if (role != null) {
//     return <Navigate to="/admin/profile" replace />;
//   }

//   return children;
// };

function AppRoutes() {
  const { isAuthenticated, role } = useAuth();
  const [isInitialLoad, setIsInitialLoad] = useState(true);

  useEffect(() => {
    if (isAuthenticated && role != null) {
      setIsInitialLoad(false);
    }
  }, [isAuthenticated, role]);

  return (
    <div className="app-wrapper">
      <Router>        
          <Header />
          <main className="main-content">
          <Routes>
            {isInitialLoad && isAuthenticated && role != null ? (
              <Route path="*" element={<Navigate to="/admin/profile" replace />} />
            ) : (
              <>
                <Route path="/" element={<Home />} />
                <Route path="/about" element={<AboutPage />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/profile" element={<CustomerProfile />}/>
                <Route path="/catalog" element={<ProductCatalog />} />
                <Route path="/product/:id" element={<ProductDetails />} />
                <Route path="/cart" element={<Cart />} />
                <Route path="/checkout" element={<Checkout />} />
                <Route path="/order-confirmation" element={<OrderConfirmation />} />
                <Route path="/search" element={<Search />} />
                <Route path="/contacts" element={<Contacts />} />
                <Route path="/help" element={<Help />} />
                <Route path="/order/:orderId" element={<OrderDetails />} />
                <Route path="/admin/profile" element={<AdminProfile />}/>
                <Route path="/admin/products" element={<ProductAdminTable />} />
                <Route path="/admin/product-edit/:productId" element={<ProductEditForm />} />
                <Route path="/admin/product-add" element={<ProductAddForm />} />
                <Route path="/admin/orders" element={<OrderAdminTable />} />
                <Route path="/admin/reviews" element={<ReviewAdminTable />} />
              </>
            )}
          </Routes>
          </main>
          <Footer />
      </Router>
    </div>
  );
}

export default AppRoutes;