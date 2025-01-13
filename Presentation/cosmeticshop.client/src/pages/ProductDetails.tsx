import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { ProductsService} from '../apiClient/http-services/products.service';
import { ReviewsService } from '../apiClient/http-services/reviews.service';
import { ProductResponseDto, ReviewResponseDto } from '../apiClient/models';

const ProductDetails: React.FC = () => {
  const { id } = useParams<{ id: string | undefined }>();
  const [product, setProduct] = useState<ProductResponseDto | null>(null);
  const [reviews, setReviews] = useState<ReviewResponseDto[] | null>(null);
  const productsService = new ProductsService('/api');
  const reviewService = new ReviewsService('/api');

  useEffect(() => {
    if (id) {
      const fetchProduct = async () => {
        try {
          const productResponse = await productsService.getProductById(id, new AbortController().signal);
          setProduct(productResponse);

          const reviewsResponse = await reviewService.getApprovedReviewsByProduct(id, new AbortController().signal);
          setReviews(reviewsResponse);
        } catch (error) {
          console.error(error);
        }
      };

      fetchProduct();
    }
  }, [id]);

  if (!product) {
    return <div>Загрузка...</div>;
  }

  return (
    <div>
      <h1>{product.name}</h1>
      <img src={product.imageUrl} alt={product.name} />
      <p>{product.description}</p>
      <p>Цена: {product.price} руб.</p>
      <button>Добавить в корзину</button>
      <h2>Отзывы</h2>
      {reviews ? 
      (<div className="reviews">
        {reviews.map((review, index) => (
          <div key={index} className="review-item">
            <p><strong>{review.reviewDate.toString()}</strong></p>
            <p><strong>{review.customerName}</strong>: {review.reviewText}</p>
            <p>Рейтинг: {review.rating}</p>
          </div>
        ))}
      </div>) : (<div>Загрузка...</div>)}
    </div>
  );
};

export default ProductDetails;