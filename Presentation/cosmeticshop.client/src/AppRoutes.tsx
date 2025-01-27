import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
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
import AboutPage from './pages/AboutPage';

function AppRoutes() {
  return (
    <Router>
        <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<AboutPage/>}/>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/profile" element={<CustomerProfile />} />
        <Route path="/catalog" element={<ProductCatalog />} />
        <Route path="/product/:id" element={<ProductDetails />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/order-confirmation" element={<OrderConfirmation />} />
        <Route path="/search" element={<Search />} />
        <Route path="/contacts" element={<Contacts />} />
        <Route path="/help" element={<Help />} />
      </Routes>
    </Router>
  );
}

export default AppRoutes;