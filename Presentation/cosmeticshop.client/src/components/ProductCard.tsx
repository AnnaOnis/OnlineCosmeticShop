import '../styles/ProductCard.css';
import '../apiClient/models/product-response-dto'
import { ProductResponseDto } from '../apiClient/models/product-response-dto';
import { useNavigate } from 'react-router-dom';


interface ProductCardProps {
    product: ProductResponseDto;
    isAuthenticated: boolean;
    isFavorited: boolean;    
    onAddToCart: () => void;
    onAddOrRemoveProductToFavorites: (event: React.MouseEvent<HTMLButtonElement>, 
                                 productId: string) => void;
  }

  const ProductCard = ({ product, 
                        isAuthenticated, 
                        onAddToCart, 
                        onAddOrRemoveProductToFavorites,
                        isFavorited }: ProductCardProps) => {

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
          <button className={`favorite-btn ${isFavorited ? 'favorited' : ''}`} 
                  disabled={!isAuthenticated}
                  onClick={(event) => onAddOrRemoveProductToFavorites(event, product.id)}>♥</button>
        </div>
        
        <div className="product-info">
          <h3 className="product-card-title" onClick={handleNavigateToProductDetails}>{product.name}</h3>
          <p className="product-card-description">{product.description}</p>
          <p className="product-rating">★ {product.rating}</p>
          <div className="product-card-footer">
            <span className="product-card-price">{product.price} руб.</span>
            <button className="btn btn-outline" onClick={onAddToCart}>В корзину</button>
          </div>
        </div>
      </div>
    );
  };

export default ProductCard;