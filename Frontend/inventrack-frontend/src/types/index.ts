// Product Management Types
export interface ProductCategory {
  CategoryId: number;
  CategoryName: string;
  Description?: string;
  ParentCategoryId?: number;
  IsActive: boolean;
  ChildCategories?: ProductCategory[];
}

export interface ProductVariant {
  variantId: number;
  productId: number;
  variantName: string;
  sku: string;
  additionalCost: number;
  isActive: boolean;
}

export interface ProductAttribute {
  attributeId: number;
  attributeName: string;
  attributeType: string;
}

export interface ProductAttributeValue {
  attributeValueId: number;
  productId: number;
  attributeId: number;
  value: string;
  attribute?: ProductAttribute;
}

export interface Product {
  ProductId: number;
  SKU: string;
  Name: string;
  Description?: string;
  CategoryId: number;
  UnitOfMeasure: string;
  Weight?: number;
  Dimensions?: string;
  UnitCost: number;
  ListPrice: number;
  MinimumStockLevel: number;
  ReorderQuantity: number;
  LeadTimeInDays: number;
  IsActive: boolean;
  CreatedDate: string;
  ModifiedDate?: string;
  ImageUrl?: string;
  Category?: ProductCategory;
  Variants?: ProductVariant[];
  AttributeValues?: ProductAttributeValue[];
}

export interface Inventory {
  inventoryId: number;
  productId: number;
  warehouseId: number;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  lastUpdated: string;
  product?: Product;
}

// Customer Management Types
export interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  registrationDate: string;
  isActive: boolean;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

export interface CustomerMembership {
  membershipId: number;
  customerId: number;
  membershipType: string;
  startDate: string;
  endDate?: string;
  isActive: boolean;
  benefits?: string;
}

// Order Management Types
export interface Order {
  orderId: number;
  customerId: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  shippingAddress?: string;
  billingAddress?: string;
  paymentMethod?: string;
  orderItems?: OrderItem[];
  customer?: Customer;
}

export interface OrderItem {
  orderItemId: number;
  orderId: number;
  productId: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  product?: Product;
}

// Supplier Management Types
export interface Supplier {
  supplierId: number;
  supplierName: string;
  contactPerson?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  isActive: boolean;
  registrationDate: string;
}

export interface PurchaseOrder {
  purchaseOrderId: number;
  supplierId: number;
  orderDate: string;
  expectedDeliveryDate?: string;
  totalAmount: number;
  status: string;
  supplier?: Supplier;
  items?: PurchaseOrderItem[];
}

export interface PurchaseOrderItem {
  purchaseOrderItemId: number;
  purchaseOrderId: number;
  productId: number;
  quantity: number;
  unitCost: number;
  totalCost: number;
  product?: Product;
}

// Warehouse Management Types
export interface Warehouse {
  warehouseId: number;
  warehouseName: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
  isActive: boolean;
  capacity?: number;
  currentUtilization?: number;
}

export interface WarehouseZone {
  zoneId: number;
  warehouseId: number;
  zoneName: string;
  zoneType: string;
  isActive: boolean;
}

// User Management Types
export interface User {
  userId: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  isActive: boolean;
  createdDate: string;
  lastLoginDate?: string;
  roles?: Role[];
}

export interface Role {
  roleId: number;
  roleName: string;
  description?: string;
  permissions?: string[];
}

// Analytics Types
export interface SalesStatistics {
  statisticsId: number;
  period: string;
  totalSales: number;
  totalOrders: number;
  averageOrderValue: number;
  topSellingProducts?: string;
  createdDate: string;
}

export interface InventoryStatistics {
  statisticsId: number;
  period: string;
  totalProducts: number;
  lowStockItems: number;
  outOfStockItems: number;
  totalValue: number;
  createdDate: string;
}

export interface DashboardWidget {
  widgetId: number;
  widgetName: string;
  widgetType: string;
  configuration?: string;
  isActive: boolean;
  displayOrder: number;
}

// API Response Types
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Form Types for Create/Update operations
export interface ProductCreateDto {
  SKU: string;
  Name: string;
  Description?: string;
  CategoryId: number;
  UnitOfMeasure: string;
  Weight?: number;
  Dimensions?: string;
  UnitCost: number;
  ListPrice: number;
  MinimumStockLevel: number;
  ReorderQuantity: number;
  LeadTimeInDays: number;
  ImageUrl?: string;
}

export interface CustomerCreateDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  country?: string;
}

export interface OrderCreateDto {
  customerId: number;
  orderItems: {
    productId: number;
    quantity: number;
    unitPrice: number;
  }[];
  shippingAddress?: string;
  billingAddress?: string;
  paymentMethod?: string;
}
