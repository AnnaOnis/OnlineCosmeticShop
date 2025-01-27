import '../styles/ProductCard.css';
import '../apiClient/models/product-response-dto'
import { ProductResponseDto } from '../apiClient/models/product-response-dto';
import { useNavigate } from 'react-router-dom';

interface ProductCardProps {
    product: ProductResponseDto;
    onAddToCart: () => void;
  }

  const ProductCard = ({ product, onAddToCart }: ProductCardProps) => {

    const navigate = useNavigate();

    const handleNavigateToProductDetails = () => {
        navigate(`/product/${product.id}`);
    };

    return (
      <div className="product-card">
        <div className="product-image-wrapper" onClick={handleNavigateToProductDetails}>
          <img 
            src={product.imageUrl} 
            alt={product.name} 
            className="product-image"
          />
          <button className="favorite-btn">♥</button>
        </div>
        
        <div className="product-info">
          <h3 className="product-card-title" onClick={handleNavigateToProductDetails}>{product.name}</h3>
          <p className="product-card-description">{product.description}</p>
          <div className="product-card-footer">
            <span className="product-card-price">{product.price} руб.</span>
            <button className="btn btn-outline" onClick={onAddToCart}>В корзину</button>
          </div>
        </div>
      </div>
    );
  };

export default ProductCard;