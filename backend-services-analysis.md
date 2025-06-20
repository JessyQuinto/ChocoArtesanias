# Análisis Completo de Servicios Backend Requeridos para Letra Creativa Generator

## Resumen del Proyecto

**Proyecto:** Plataforma de comercio electrónico para artesanías del Chocó (Colombia)

**Tecnologías Frontend:** React 18, TypeScript, Vite, Zustand, React Router, shadcn/ui, TanStack Query

**Estado Actual:** Frontend desarrollado con datos mock, sin integración backend real

---

## 1. SERVICIOS DE AUTENTICACIÓN Y AUTORIZACIÓN

### 1.1 Registro de Usuario
- **Ruta:** `POST /api/auth/register`
- **Método:** POST
- **Descripción:** Registro de nuevos usuarios en la plataforma

**JSON de Entrada:**
```json
{
  "firstName": "string",
  "lastName": "string", 
  "email": "string",
  "password": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "token": "string",
    "user": {
      "id": "string",
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "role": "customer"
    }
  },
  "message": "Usuario registrado exitosamente"
}
```

### 1.2 Inicio de Sesión
- **Ruta:** `POST /api/auth/login`
- **Método:** POST
- **Descripción:** Autenticación de usuarios existentes

**JSON de Entrada:**
```json
{
  "email": "string",
  "password": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "token": "string",
    "user": {
      "id": "string",
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "role": "customer",
      "addresses": []
    }
  },
  "message": "Inicio de sesión exitoso"
}
```

### 1.3 Cerrar Sesión
- **Ruta:** `POST /api/auth/logout`
- **Método:** POST
- **Descripción:** Invalidar token de usuario

**JSON de Entrada:**
```json
{
  "token": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "message": "Sesión cerrada exitosamente"
}
```

### 1.4 Renovar Token
- **Ruta:** `POST /api/auth/refresh`
- **Método:** POST
- **Descripción:** Renovar token de acceso

**JSON de Entrada:**
```json
{
  "refreshToken": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "token": "string",
    "refreshToken": "string"
  }
}
```

---

## 2. SERVICIOS DE GESTIÓN DE USUARIOS

### 2.1 Obtener Perfil de Usuario
- **Ruta:** `GET /api/users/profile`
- **Método:** GET
- **Descripción:** Obtener datos del perfil del usuario autenticado

**JSON de Entrada:** N/A (Headers: Authorization Bearer token)

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phone": "string",
    "addresses": [
      {
        "id": "string",
        "name": "string",
        "fullName": "string",
        "address": "string",
        "city": "string",
        "postalCode": "string",
        "phone": "string",
        "isDefault": true
      }
    ],
    "createdAt": "string",
    "updatedAt": "string"
  }
}
```

### 2.2 Actualizar Perfil de Usuario
- **Ruta:** `PUT /api/users/profile`
- **Método:** PUT
- **Descripción:** Actualizar datos del perfil del usuario

**JSON de Entrada:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "phone": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phone": "string"
  },
  "message": "Perfil actualizado exitosamente"
}
```

### 2.3 Agregar Dirección
- **Ruta:** `POST /api/users/addresses`
- **Método:** POST
- **Descripción:** Agregar nueva dirección al usuario

**JSON de Entrada:**
```json
{
  "name": "string",
  "fullName": "string",
  "address": "string",
  "city": "string",
  "postalCode": "string",
  "phone": "string",
  "isDefault": false
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "name": "string",
    "fullName": "string",
    "address": "string",
    "city": "string",
    "postalCode": "string",
    "phone": "string",
    "isDefault": false
  },
  "message": "Dirección agregada exitosamente"
}
```

### 2.4 Actualizar Dirección
- **Ruta:** `PUT /api/users/addresses/{addressId}`
- **Método:** PUT
- **Descripción:** Actualizar dirección existente

**JSON de Entrada:**
```json
{
  "name": "string",
  "fullName": "string",
  "address": "string",
  "city": "string",
  "postalCode": "string",
  "phone": "string",
  "isDefault": false
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "name": "string",
    "fullName": "string",
    "address": "string",
    "city": "string",
    "postalCode": "string",
    "phone": "string",
    "isDefault": false
  },
  "message": "Dirección actualizada exitosamente"
}
```

### 2.5 Eliminar Dirección
- **Ruta:** `DELETE /api/users/addresses/{addressId}`
- **Método:** DELETE
- **Descripción:** Eliminar dirección del usuario

**JSON de Entrada:** N/A

**JSON de Salida:**
```json
{
  "success": true,
  "message": "Dirección eliminada exitosamente"
}
```

---

## 3. SERVICIOS DE PRODUCTOS

### 3.1 Listar Productos
- **Ruta:** `GET /api/products`
- **Método:** GET
- **Descripción:** Obtener lista de productos con filtros y paginación

**Parámetros de Query:**
- page: number (default: 1)
- limit: number (default: 20)
- category: string
- search: string
- minPrice: number
- maxPrice: number
- artisan: string
- origin: string
- featured: boolean
- inStock: boolean
- sortBy: string (price, name, date)
- sortOrder: string (asc, desc)

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "products": [
      {
        "id": 1,
        "name": "string",
        "slug": "string",
        "description": "string",
        "price": 0,
        "discountedPrice": 0,
        "image": "string",
        "images": ["string"],
        "categoryId": 1,
        "category": {
          "id": 1,
          "name": "string",
          "slug": "string"
        },
        "producerId": 1,
        "producer": {
          "id": 1,
          "name": "string",
          "location": "string"
        },
        "stock": 0,
        "featured": true,
        "rating": 0,
        "artisan": "string",
        "origin": "string",
        "createdAt": "string",
        "updatedAt": "string"
      }
    ],
    "pagination": {
      "currentPage": 1,
      "totalPages": 10,
      "totalItems": 200,
      "itemsPerPage": 20
    }
  }
}
```

### 3.2 Obtener Producto por ID/Slug
- **Ruta:** `GET /api/products/{slug}`
- **Método:** GET
- **Descripción:** Obtener detalles específicos de un producto

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "slug": "string",
    "description": "string",
    "price": 0,
    "discountedPrice": 0,
    "image": "string",
    "images": ["string"],
    "categoryId": 1,
    "category": {
      "id": 1,
      "name": "string",
      "slug": "string",
      "description": "string"
    },
    "producerId": 1,
    "producer": {
      "id": 1,
      "name": "string",
      "description": "string",
      "location": "string",
      "image": "string",
      "foundationYear": 2000
    },
    "stock": 0,
    "featured": true,
    "rating": 0,
    "artisan": "string",
    "origin": "string",
    "story": {
      "id": 1,
      "title": "string",
      "content": "string",
      "author": "string",
      "readTime": "string",
      "culturalSignificance": "string"
    },
    "reviews": [
      {
        "id": 1,
        "userId": "string",
        "userName": "string",
        "rating": 5,
        "comment": "string",
        "createdAt": "string"
      }
    ],
    "createdAt": "string",
    "updatedAt": "string"
  }
}
```

### 3.3 Productos Destacados
- **Ruta:** `GET /api/products/featured`
- **Método:** GET
- **Descripción:** Obtener productos marcados como destacados

**JSON de Salida:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "slug": "string",
      "description": "string",
      "price": 0,
      "discountedPrice": 0,
      "image": "string",
      "categoryId": 1,
      "producerId": 1,
      "stock": 0,
      "featured": true,
      "rating": 0,
      "artisan": "string",
      "origin": "string"
    }
  ]
}
```

### 3.4 Búsqueda de Productos
- **Ruta:** `GET /api/products/search`
- **Método:** GET
- **Descripción:** Búsqueda avanzada de productos

**Parámetros de Query:**
- q: string (término de búsqueda)
- category: string
- minPrice: number
- maxPrice: number
- artisan: string
- region: string
- inStock: boolean

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "products": [],
    "totalResults": 0,
    "suggestions": ["string"]
  }
}
```

---

## 4. SERVICIOS DE CATEGORÍAS

### 4.1 Listar Categorías
- **Ruta:** `GET /api/categories`
- **Método:** GET
- **Descripción:** Obtener todas las categorías disponibles

**JSON de Salida:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "slug": "string",
      "description": "string",
      "image": "string",
      "productCount": 0
    }
  ]
}
```

### 4.2 Obtener Categoría
- **Ruta:** `GET /api/categories/{slug}`
- **Método:** GET
- **Descripción:** Obtener detalles de una categoría específica

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "slug": "string",
    "description": "string",
    "image": "string",
    "products": [],
    "productCount": 0
  }
}
```

---

## 5. SERVICIOS DE PRODUCTORES/ARTESANOS

### 5.1 Listar Productores
- **Ruta:** `GET /api/producers`
- **Método:** GET
- **Descripción:** Obtener lista de productores/artesanos

**JSON de Salida:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "description": "string",
      "location": "string",
      "image": "string",
      "featured": true,
      "foundationYear": 2000,
      "productCount": 0
    }
  ]
}
```

### 5.2 Obtener Productor
- **Ruta:** `GET /api/producers/{id}`
- **Método:** GET
- **Descripción:** Obtener detalles de un productor específico

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "location": "string",
    "image": "string",
    "featured": true,
    "foundationYear": 2000,
    "products": [],
    "stories": [],
    "contact": {
      "phone": "string",
      "email": "string",
      "address": "string"
    }
  }
}
```

---

## 6. SERVICIOS DE CARRITO DE COMPRAS

### 6.1 Obtener Carrito
- **Ruta:** `GET /api/cart`
- **Método:** GET
- **Descripción:** Obtener carrito del usuario autenticado

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "userId": "string",
    "items": [
      {
        "id": "string",
        "productId": 1,
        "product": {
          "id": 1,
          "name": "string",
          "image": "string",
          "price": 0,
          "stock": 0
        },
        "quantity": 1,
        "unitPrice": 0,
        "totalPrice": 0
      }
    ],
    "itemCount": 0,
    "subtotal": 0,
    "tax": 0,
    "total": 0,
    "updatedAt": "string"
  }
}
```

### 6.2 Agregar al Carrito
- **Ruta:** `POST /api/cart/items`
- **Método:** POST
- **Descripción:** Agregar producto al carrito

**JSON de Entrada:**
```json
{
  "productId": 1,
  "quantity": 1
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "productId": 1,
    "quantity": 1,
    "unitPrice": 0,
    "totalPrice": 0
  },
  "message": "Producto agregado al carrito"
}
```

### 6.3 Actualizar Cantidad en Carrito
- **Ruta:** `PUT /api/cart/items/{itemId}`
- **Método:** PUT
- **Descripción:** Actualizar cantidad de un producto en el carrito

**JSON de Entrada:**
```json
{
  "quantity": 2
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "productId": 1,
    "quantity": 2,
    "unitPrice": 0,
    "totalPrice": 0
  },
  "message": "Cantidad actualizada"
}
```

### 6.4 Eliminar del Carrito
- **Ruta:** `DELETE /api/cart/items/{itemId}`
- **Método:** DELETE
- **Descripción:** Eliminar producto del carrito

**JSON de Salida:**
```json
{
  "success": true,
  "message": "Producto eliminado del carrito"
}
```

### 6.5 Vaciar Carrito
- **Ruta:** `DELETE /api/cart`
- **Método:** DELETE
- **Descripción:** Vaciar completamente el carrito

**JSON de Salida:**
```json
{
  "success": true,
  "message": "Carrito vaciado"
}
```

---

## 7. SERVICIOS DE LISTA DE DESEOS (WISHLIST)

### 7.1 Obtener Lista de Deseos
- **Ruta:** `GET /api/wishlist`
- **Método:** GET
- **Descripción:** Obtener lista de deseos del usuario

**JSON de Salida:**
```json
{
  "success": true,
  "data": [
    {
      "id": "string",
      "productId": 1,
      "product": {
        "id": 1,
        "name": "string",
        "slug": "string",
        "price": 0,
        "image": "string",
        "stock": 0
      },
      "addedAt": "string"
    }
  ]
}
```

### 7.2 Agregar a Lista de Deseos
- **Ruta:** `POST /api/wishlist`
- **Método:** POST
- **Descripción:** Agregar producto a lista de deseos

**JSON de Entrada:**
```json
{
  "productId": 1
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "productId": 1,
    "addedAt": "string"
  },
  "message": "Producto agregado a lista de deseos"
}
```

### 7.3 Eliminar de Lista de Deseos
- **Ruta:** `DELETE /api/wishlist/{productId}`
- **Método:** DELETE
- **Descripción:** Eliminar producto de lista de deseos

**JSON de Salida:**
```json
{
  "success": true,
  "message": "Producto eliminado de lista de deseos"
}
```

---

## 8. SERVICIOS DE PEDIDOS (ORDERS)

### 8.1 Crear Pedido
- **Ruta:** `POST /api/orders`
- **Método:** POST
- **Descripción:** Crear nuevo pedido desde el carrito

**JSON de Entrada:**
```json
{
  "shippingAddressId": "string",
  "paymentMethod": "credit-card",
  "paymentDetails": {
    "cardNumber": "string",
    "expiryDate": "string",
    "cvv": "string",
    "cardHolderName": "string"
  },
  "notes": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "orderNumber": "string",
    "userId": "string",
    "status": "pending",
    "items": [
      {
        "id": "string",
        "productId": 1,
        "product": {
          "id": 1,
          "name": "string",
          "image": "string"
        },
        "quantity": 1,
        "unitPrice": 0,
        "totalPrice": 0
      }
    ],
    "shippingAddress": {
      "fullName": "string",
      "address": "string",
      "city": "string",
      "postalCode": "string",
      "phone": "string"
    },
    "paymentMethod": "credit-card",
    "subtotal": 0,
    "tax": 0,
    "shipping": 0,
    "total": 0,
    "createdAt": "string",
    "estimatedDelivery": "string"
  },
  "message": "Pedido creado exitosamente"
}
```

### 8.2 Obtener Pedidos del Usuario
- **Ruta:** `GET /api/orders`
- **Método:** GET
- **Descripción:** Obtener historial de pedidos del usuario

**Parámetros de Query:**
- page: number
- limit: number
- status: string

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "orders": [
      {
        "id": "string",
        "orderNumber": "string",
        "status": "delivered",
        "total": 0,
        "itemCount": 3,
        "createdAt": "string",
        "estimatedDelivery": "string"
      }
    ],
    "pagination": {
      "currentPage": 1,
      "totalPages": 5,
      "totalItems": 50
    }
  }
}
```

### 8.3 Obtener Detalles de Pedido
- **Ruta:** `GET /api/orders/{orderId}`
- **Método:** GET
- **Descripción:** Obtener detalles completos de un pedido

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "orderNumber": "string",
    "userId": "string",
    "status": "processing",
    "items": [
      {
        "id": "string",
        "productId": 1,
        "product": {
          "id": 1,
          "name": "string",
          "image": "string",
          "slug": "string"
        },
        "quantity": 1,
        "unitPrice": 0,
        "totalPrice": 0
      }
    ],
    "shippingAddress": {
      "fullName": "string",
      "address": "string",
      "city": "string",
      "postalCode": "string",
      "phone": "string"
    },
    "paymentMethod": "credit-card",
    "paymentStatus": "paid",
    "subtotal": 0,
    "tax": 0,
    "shipping": 0,
    "total": 0,
    "statusHistory": [
      {
        "status": "pending",
        "timestamp": "string",
        "note": "string"
      }
    ],
    "tracking": {
      "carrier": "string",
      "trackingNumber": "string",
      "estimatedDelivery": "string"
    },
    "createdAt": "string",
    "updatedAt": "string"
  }
}
```

### 8.4 Cancelar Pedido
- **Ruta:** `PUT /api/orders/{orderId}/cancel`
- **Método:** PUT
- **Descripción:** Cancelar pedido (solo si está en estado 'pending' o 'processing')

**JSON de Entrada:**
```json
{
  "reason": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "status": "canceled",
    "cancelReason": "string",
    "canceledAt": "string"
  },
  "message": "Pedido cancelado exitosamente"
}
```

---

## 9. SERVICIOS DE RESEÑAS Y CALIFICACIONES

### 9.1 Obtener Reseñas de Producto
- **Ruta:** `GET /api/products/{productId}/reviews`
- **Método:** GET
- **Descripción:** Obtener reseñas de un producto específico

**Parámetros de Query:**
- page: number
- limit: number
- rating: number (filtrar por calificación)

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "reviews": [
      {
        "id": "string",
        "userId": "string",
        "user": {
          "name": "string",
          "avatar": "string"
        },
        "rating": 5,
        "title": "string",
        "comment": "string",
        "helpful": 0,
        "verified": true,
        "createdAt": "string"
      }
    ],
    "summary": {
      "averageRating": 4.5,
      "totalReviews": 150,
      "ratingDistribution": {
        "5": 80,
        "4": 45,
        "3": 15,
        "2": 7,
        "1": 3
      }
    },
    "pagination": {
      "currentPage": 1,
      "totalPages": 10,
      "totalItems": 150
    }
  }
}
```

### 9.2 Crear Reseña
- **Ruta:** `POST /api/products/{productId}/reviews`
- **Método:** POST
- **Descripción:** Crear nueva reseña para un producto

**JSON de Entrada:**
```json
{
  "rating": 5,
  "title": "string",
  "comment": "string"
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": "string",
    "productId": 1,
    "userId": "string",
    "rating": 5,
    "title": "string",
    "comment": "string",
    "createdAt": "string"
  },
  "message": "Reseña creada exitosamente"
}
```

### 9.3 Marcar Reseña como Útil
- **Ruta:** `POST /api/reviews/{reviewId}/helpful`
- **Método:** POST
- **Descripción:** Marcar una reseña como útil

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "reviewId": "string",
    "helpful": true,
    "helpfulCount": 15
  },
  "message": "Reseña marcada como útil"
}
```

---

## 10. SERVICIOS ADMINISTRATIVOS

### 10.1 Dashboard de Administración
- **Ruta:** `GET /api/admin/dashboard`
- **Método:** GET
- **Descripción:** Obtener estadísticas generales para el dashboard

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "stats": {
      "totalUsers": 1234,
      "totalProducts": 156,
      "totalOrders": 890,
      "totalRevenue": 45000000,
      "ordersToday": 45,
      "pendingOrders": 12
    },
    "recentOrders": [],
    "topProducts": [],
    "salesChart": {
      "labels": ["string"],
      "data": [0]
    }
  }
}
```

### 10.2 Gestionar Productos (Admin)
- **Ruta:** `POST /api/admin/products`
- **Método:** POST
- **Descripción:** Crear nuevo producto (solo administradores)

**JSON de Entrada:**
```json
{
  "name": "string",
  "description": "string",
  "price": 0,
  "discountedPrice": 0,
  "categoryId": 1,
  "producerId": 1,
  "stock": 0,
  "featured": false,
  "artisan": "string",
  "origin": "string",
  "images": ["string"]
}
```

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "slug": "string",
    "description": "string",
    "price": 0,
    "stock": 0,
    "createdAt": "string"
  },
  "message": "Producto creado exitosamente"
}
```

### 10.3 Actualizar Producto (Admin)
- **Ruta:** `PUT /api/admin/products/{productId}`
- **Método:** PUT
- **Descripción:** Actualizar producto existente

### 10.4 Eliminar Producto (Admin)
- **Ruta:** `DELETE /api/admin/products/{productId}`
- **Método:** DELETE
- **Descripción:** Eliminar producto

### 10.5 Gestionar Pedidos (Admin)
- **Ruta:** `GET /api/admin/orders`
- **Método:** GET
- **Descripción:** Obtener todos los pedidos para administración

### 10.6 Actualizar Estado de Pedido (Admin)
- **Ruta:** `PUT /api/admin/orders/{orderId}/status`
- **Método:** PUT
- **Descripción:** Actualizar estado de un pedido

**JSON de Entrada:**
```json
{
  "status": "processing",
  "note": "string",
  "trackingNumber": "string"
}
```

---

## 11. SERVICIOS DE HISTORIAS CULTURALES

### 11.1 Obtener Historias
- **Ruta:** `GET /api/stories`
- **Método:** GET
- **Descripción:** Obtener historias culturales y artesanales

**JSON de Salida:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "string",
      "excerpt": "string",
      "content": "string",
      "author": "string",
      "readTime": "string",
      "culturalSignificance": "string",
      "image": "string",
      "relatedProducts": [],
      "createdAt": "string"
    }
  ]
}
```

### 11.2 Obtener Historia por ID
- **Ruta:** `GET /api/stories/{storyId}`
- **Método:** GET
- **Descripción:** Obtener detalles de una historia específica

---

## 12. SERVICIOS DE BÚSQUEDA Y FILTROS

### 12.1 Autocompletado de Búsqueda
- **Ruta:** `GET /api/search/autocomplete`
- **Método:** GET
- **Descripción:** Obtener sugerencias para autocompletado

**Parámetros de Query:**
- q: string (término de búsqueda)

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "suggestions": ["string"],
    "categories": ["string"],
    "artisans": ["string"]
  }
}
```

### 12.2 Filtros Disponibles
- **Ruta:** `GET /api/search/filters`
- **Método:** GET
- **Descripción:** Obtener filtros disponibles para búsqueda

**JSON de Salida:**
```json
{
  "success": true,
  "data": {
    "categories": [
      {
        "id": 1,
        "name": "string",
        "count": 25
      }
    ],
    "priceRanges": [
      {
        "label": "Menos de $50,000",
        "min": 0,
        "max": 50000,
        "count": 45
      }
    ],
    "artisans": ["string"],
    "regions": ["string"]
  }
}
```

---

## ANÁLISIS DE PARTES FALTANTES DEL FRONTEND

### 1. CONEXIONES DE SERVICIOS FALTANTES

#### 1.1 Autenticación Real
**Estado Actual:** Mock authentication en `AuthContext.tsx` (líneas 89-104)
**Faltante:**
- Reemplazar simulación en `loginMutation.mutationFn` con llamada HTTP real a `POST /api/auth/login`
- Implementar registro real en lugar de mock en `registerMutation.mutationFn`
- Agregar manejo de tokens JWT y refresh tokens
- Implementar interceptores HTTP para agregar tokens automáticamente

**Archivo:** `src/context/AuthContext.tsx`
**Cambios Requeridos:**
```typescript
// Línea 89-104: Reemplazar mock con API real
mutationFn: async ({ email, password }) => {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  
  if (!response.ok) {
    throw new Error('Credenciales incorrectas');
  }
  
  return response.json();
}
```

#### 1.2 Gestión de Productos
**Estado Actual:** Usando datos mock en `productApi.ts`
**Faltante:**
- Reemplazar `fetchFeaturedProducts()` con llamada real a `GET /api/products/featured`
- Implementar `fetchProductBySlug()` con API real
- Agregar funciones para filtros y búsqueda avanzada

**Archivo:** `src/services/productApi.ts`
**Cambios Requeridos:**
- Eliminar mock data y reemplazar con HTTP requests
- Implementar funciones de filtrado y paginación

#### 1.3 Carrito de Compras
**Estado Actual:** Solo estado local en Zustand
**Faltante:**
- Sincronización con backend para usuarios autenticados
- Persistencia del carrito en servidor
- Implementar llamadas a endpoints de carrito

**Archivo:** `src/store/useStore.ts` (líneas 125-196)
**Cambios Requeridos:**
```typescript
// Agregar funciones para sincronizar con backend
addToCart: async (item) => {
  // Estado local actual...
  
  // Agregar sincronización con backend
  if (get().auth.isLoggedIn) {
    await fetch('/api/cart/items', {
      method: 'POST',
      headers: { 
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${get().auth.token}`
      },
      body: JSON.stringify({
        productId: item.id,
        quantity: item.quantity
      })
    });
  }
}
```

#### 1.4 Proceso de Checkout
**Estado Actual:** Simulación local en `Checkout.tsx`
**Faltante:**
- Integrar con API de creación de pedidos `POST /api/orders`
- Implementar validación de inventario en tiempo real
- Agregar procesamiento de pagos real

**Archivo:** `src/pages/Checkout.tsx` (líneas 85-95)
**Cambios Requeridos:**
```typescript
const handlePlaceOrder = async () => {
  if (!validateForm() || cartItems.length === 0) return;

  setProcessing(true);
  
  try {
    const response = await fetch('/api/orders', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${auth.token}`
      },
      body: JSON.stringify({
        shippingAddressId: selectedAddressId,
        paymentMethod,
        paymentDetails: cardInfo
      })
    });
    
    if (response.ok) {
      const order = await response.json();
      completeOrder();
      navigate(`/order-confirmation?order_id=${order.data.orderNumber}`);
    }
  } catch (error) {
    setErrors({ general: 'Error al procesar el pedido' });
  } finally {
    setProcessing(false);
  }
};
```

### 2. FUNCIONALIDADES INCOMPLETAS

#### 2.1 Sistema de Reseñas
**Estado Actual:** Solo UI básica sin funcionalidad
**Faltante:**
- Implementar formulario para crear reseñas
- Mostrar reseñas existentes en `ProductDetail.tsx`
- Agregar sistema de calificaciones

**Archivos Afectados:**
- `src/components/ProductReviews.tsx` (falta implementación completa)
- `src/pages/ProductDetail.tsx` (falta integración con reseñas)

#### 2.2 Lista de Deseos (Wishlist)
**Estado Actual:** Solo estado local en Zustand
**Faltante:**
- Sincronización con backend para persistencia
- Página dedicada para ver/gestionar wishlist
- Notificaciones cuando productos en wishlist tienen descuento

**Archivo:** `src/store/useStore.ts` (líneas 322-332)
**Cambios Requeridos:**
- Agregar llamadas HTTP para sincronizar wishlist
- Crear página `Wishlist.tsx`

#### 2.3 Gestión de Pedidos
**Estado Actual:** No existe visualización de pedidos
**Faltante:**
- Página de historial de pedidos del usuario
- Detalles de pedido individual
- Seguimiento de pedidos

**Archivos a Crear:**
- `src/pages/OrderHistory.tsx`
- `src/components/OrderCard.tsx`
- `src/components/OrderTracking.tsx`

#### 2.4 Panel de Administración
**Estado Actual:** UI básica sin funcionalidad real
**Faltante:**
- Conexión con APIs administrativas
- Gestión de productos (CRUD completo)
- Gestión de pedidos y usuarios
- Reportes y estadísticas

**Archivo:** `src/pages/AdminPanel.tsx` (líneas 38-50)
**Cambios Requeridos:**
- Implementar llamadas a endpoints administrativos
- Agregar formularios de gestión de productos
- Implementar dashboard con estadísticas reales

#### 2.5 Búsqueda Avanzada
**Estado Actual:** Solo filtros básicos locales
**Faltante:**
- Integración con API de búsqueda
- Autocompletado en tiempo real
- Filtros dinámicos basados en datos del servidor

**Archivo:** `src/components/search/AdvancedSearch.tsx`
**Cambios Requeridos:**
- Implementar debounced search con API
- Agregar filtros de precio, región, artesano
- Implementar sugerencias de búsqueda

### 3. FUNCIONALIDADES COMPLETAMENTE FALTANTES

#### 3.1 Sistema de Notificaciones
**Faltante:**
- Notificaciones push para actualizaciones de pedidos
- Alertas de stock bajo para productos en wishlist
- Notificaciones promocionales

**Archivos a Crear:**
- `src/services/notificationService.ts`
- `src/components/NotificationCenter.tsx`

#### 3.2 Chat/Soporte al Cliente
**Faltante:**
- Sistema de mensajería con soporte
- Chat en vivo o tickets de soporte
- FAQ dinámico

#### 3.3 Sistema de Cupones y Descuentos
**Faltante:**
- Aplicación de códigos de descuento en checkout
- Gestión de promociones
- Cálculo de descuentos automáticos

#### 3.4 Análisis y Métricas
**Faltante:**
- Tracking de eventos de usuario
- Analíticas de productos más vistos
- Reportes de ventas

#### 3.5 Integración de Pagos
**Faltante:**
- Integración con pasarelas de pago (PSE, tarjetas)
- Procesamiento de pagos real
- Manejo de estados de pago

### 4. OPTIMIZACIONES NECESARIAS

#### 4.1 Manejo de Estados de Carga
**Faltante:**
- Loading states más granulares
- Error boundaries específicos
- Retry logic para requests fallidos

#### 4.2 Cache y Performance
**Faltante:**
- Implementar cache inteligente con TanStack Query
- Optimización de imágenes
- Lazy loading de componentes

#### 4.3 Accesibilidad
**Faltante:**
- ARIA labels adecuados
- Navegación por teclado
- Soporte para lectores de pantalla

### 5. SEGURIDAD

#### 5.1 Validación de Datos
**Faltante:**
- Validación más robusta en formularios
- Sanitización de inputs
- Protección contra XSS

#### 5.2 Autenticación y Autorización
**Faltante:**
- Refresh automático de tokens
- Logout automático en expiración
- Protección de rutas administrativas

---

## PRIORIDADES DE IMPLEMENTACIÓN

### Prioridad Alta (Funcionalidad Básica)
1. Implementar autenticación real con JWT
2. Conectar productos con API real
3. Implementar carrito persistente
4. Completar proceso de checkout con API
5. Agregar gestión básica de pedidos

### Prioridad Media (Mejoras de UX)
1. Sistema de reseñas y calificaciones
2. Lista de deseos persistente
3. Búsqueda avanzada con API
4. Historial de pedidos de usuario
5. Panel administrativo funcional

### Prioridad Baja (Características Avanzadas)
1. Sistema de notificaciones
2. Chat de soporte
3. Análisis y métricas
4. Sistema de cupones
5. Integración de pagos avanzada

---

## CONCLUSIÓN

El frontend está bien estructurado con una base sólida usando React, TypeScript y librerías modernas. Sin embargo, **todas las funcionalidades actualmente usan datos mock y no están conectadas a servicios backend reales**. 

Los servicios backend listados anteriormente son **esenciales** para convertir esta aplicación de una demo con datos falsos a una plataforma de e-commerce completamente funcional.

El mayor trabajo faltante está en:
1. **Reemplazar todas las simulaciones con llamadas HTTP reales**
2. **Implementar manejo adecuado de autenticación con tokens**
3. **Agregar funcionalidades faltantes como reseñas, historial de pedidos, y panel administrativo**
4. **Implementar persistencia de datos del lado del servidor**

Una vez implementados los servicios backend, el frontend requerirá refactoring significativo para eliminar los mocks y conectar con las APIs reales.
