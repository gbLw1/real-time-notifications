export interface NotificationModel {
  id: string;
  content: string;
  redirectUrl: string | null;
  isRead: boolean;
}
