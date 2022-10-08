using HolyBot.Razebator.data;
using HolyBot.Razebator.level;
using HolyBot.Razebator.math;
using HolyBot.Razebator.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class PhysicsControler : BotControler {
        public State state = State.waitForPosPacket;
        public Vector3D before;
        public float beforeYaw;
        public float beforePitch;
        public Bot client;
        public int sleepticks = 0;
        private int autojumpcooldown = 0;
        private Block blockUnder = null;
        private bool WALK = false;
        private bool RUN = false;
        private bool SNEAK = false;
        bool xzcollided = false;
        private bool jumpQueued;

        public PhysicsControler(Bot client) {
            this.client = client;
        }

        public void Dispose() {
            
        }

        public void tick() {
            if (state == State.ready) {
                if (!client.isOnline()) {
                    state = State.waitForPosPacket;
                    return;
                }

                PhysicsUpdate();
            }
        }

        private void airfall() {
            client.velY -= physics.gravity;
            client.velY *= physics.airdrag;
        }

        private void waterfall() {
            client.velY *= client.isInWater() ? physics.waterInertia : physics.lavaInertia;
            client.velY -= client.isInWater() ? physics.waterGravity : physics.lavaGravity;
        }

        private AABB nexttickX() {
            return client.getHitbox().offset(client.velX, 0, 0);
        }

        private AABB nexttickZ() {
            return client.getHitbox().offset(0, 0, client.velZ);
        }

        private AABB nexttickY() {
            return client.getHitbox().offset(0, client.velY, 0);
        }

        public void jump() {
            this.jumpQueued = true;
        }

        public Block getBlockPosBelowThatAffectsMyMovement() {
            return client.getWorld().getBlock(client.posX, client.posY - 0.5000001, client.posZ);
        }

        public void Walk() {
            WALK = true;
        }

        public void Sprint() {
            WALK = true;
            RUN = true;
        }

        private double getMoveSpeed() {
            if (RUN)
                return 0.38985D;
            else if (WALK)
                return 0.3473D;
            else
                return 0D;
        }

        public void moveRelative(double forward, double strafe, double friction) {
            double distance = strafe * strafe + forward * forward;

            if (distance >= 1.0E-4F) {
                distance = Math.Sqrt(distance);

                if (distance < 1.0F) {
                    distance = 1.0F;
                }

                distance = friction / distance;
                strafe = strafe * distance;
                forward = forward * distance;

                double yawRadians = MathU.toRadians(client.getYaw());
                double sin = Math.Sin(yawRadians);
                double cos = Math.Cos(yawRadians);

                client.velX += strafe * cos - forward * sin;
                client.velZ += forward * cos + strafe * sin;
            }
        }

        public void moveEntityWithHeading(double forward, double strafe) {
            float prevSlipperiness = (float)physics.airborneInertia;//inertia
            double value = physics.airborneAcceleration;//acceleration

            if (!client.isInLiquid()) {
                if (client.onGround) {
                    prevSlipperiness = (blockUnder.getfriction() == 0.6F ? getBlockPosBelowThatAffectsMyMovement().getfriction() : blockUnder.getfriction()) * 0.91F;//inertiа
                    value = getMoveSpeed() * (0.1627714F / (prevSlipperiness * prevSlipperiness * prevSlipperiness));//acceleration
                }

                client.velX *= prevSlipperiness;
                client.velZ *= prevSlipperiness;

                moveRelative(forward, strafe, value);//apply heading

            } else {
                double acceleration = physics.liquidAcceleration;
                double inertia = client.isInWater() ? physics.waterInertia : physics.lavaInertia;
                double horizontalInertia = inertia;

                moveRelative(strafe, forward, acceleration);

                client.velY *= inertia;
                client.velY -= client.isInWater() ? physics.waterGravity : physics.lavaGravity;// * gravityMultiplier;
                client.velX *= horizontalInertia;
                client.velZ *= horizontalInertia;
            }

        }

        public void walksAndOtherShit() {
            if (Math.Abs(client.velX) < physics.negligeableVelocity)
                client.velX = 0;
            if (Math.Abs(client.velY) < physics.negligeableVelocity)
                client.velY = 0;
            if (Math.Abs(client.velZ) < physics.negligeableVelocity)
                client.velZ = 0;

            if (jumpQueued && autojumpcooldown <= 0) {
                if (client.isInLiquid()) {
                    client.velY += 0.03999999910593033F;
                    autojumpcooldown = 1;
                } else {
                    if (client.onGround) {
                        client.velY = 0.5099999904632568F;
                        if (client.effects.jumpBoost > 0) {
                            client.velY += 0.1f * (float)(client.effects.jumpBoost + 1);
                        }
                        if (RUN) {
                            float yaw = client.getYaw() * 0.017453292f;
                            vel.add(-Math.Sin(yaw) * 0.2f, 0.0, Math.Cos(yaw) * 0.2f);
                        }
                    }
                }
                jumpQueued = false;
            }

            double strafe = 0;
            double forward = (RUN || WALK) ? getMoveSpeed() : 0 * 0.98;

            if (SNEAK || client.isHoldSlowdownItem) {
                strafe *= physics.sneakSpeed;
                forward *= physics.sneakSpeed;
            }

            moveEntityWithHeading(forward, strafe);
        }

        private void PhysicsUpdate() {
            if (sleepticks > 0) {
                sleepticks--;
                return;
            }
            xzcollided = false;

            blockUnder = client.getWorld().getBlock(client.getPosition().floor().add(0, -1, 0));
            if (client.foodlvl <= 6)
                RUN = false;

            walksAndOtherShit();

            RUN = false;
            WALK = false;
            SNEAK = false;

            if (client.isInLiquid()) {
                waterfall();
            } else {
                airfall();
            }
            client.onGround = false;

            if (client.velY != 0) {
                for (Vector3D a : client.getHitbox(0, client.velY, 0).getCorners()) {
                    Block n = a.func_vf().getBlock(client);
                    if (n.getHitbox() != null && !n.isLiquid()) {
                        if (n.getHitbox().collide(nexttickY())) {
                            if (client.velY > 0) {
                                if (n.getHitbox().minY < client.getHitbox(vel).maxY) {
                                    client.velY = 0;
                                    client.setPosY(client.getPosY() + (n.getHitbox().minY - client.getHitbox().maxY));
                                }
                            } else {
                                if (n.getHitbox().maxY > client.posY + client.velY) {
                                    client.velY = 0;
                                    client.setPosY(n.getHitbox().maxY);
                                    client.onGround = true;
                                } else {
                                    client.velY = 0;
                                    client.onGround = true;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            //bad
            if (client.velX != 0) {
                for (Vector3D a : client.getHitbox(client.velX, 0, 0).getCorners()) {
                    Block n = a.func_vf().getBlock(client);
                    if (n.getHitbox() != null && !n.isLiquid()) {
                        if (n.getHitbox().collide(nexttickX())) {
                            xzcollided = true;
                            //System.out.println(n.getHitbox().maxY - Math.Floor(client.posY));
                            if (n.getHitbox().maxY - Math.Floor(client.posY) <= physics.stepHeight) {
                                client.velY = 0;
                                client.setPosY(n.getHitbox().maxY);
                            } else if (a.up().getBlock(client).isAvoid() && Math.Floor(a.y) == Math.Floor(client.posY)) {
                                client.velX = 0;
                                jump();
                            } else {
                                client.velX = 0;
                            }
                            break;
                        }
                    }
                }
            }

            if (client.velZ != 0) {
                for (Vector3D a : client.getHitbox(0, 0, client.velZ).getCorners()) {
                    Block n = a.func_vf().getBlock(client);
                    if (n.getHitbox() != null && !n.isLiquid()) {
                        if (n.getHitbox().collide(nexttickZ())) {
                            xzcollided = true;
                            if (n.getHitbox().maxY - Math.Floor(client.posY) <= physics.stepHeight) {
                                client.velY = 0;
                                client.setPosY(n.getHitbox().maxY);
                            } else if (a.up().getBlock(client).isAvoid() && Math.Floor(a.y) == Math.Floor(client.posY)) {
                                client.velZ = 0;
                                jump();
                            } else {
                                client.velZ = 0;
                            }
                            break;
                        }
                    }
                }
            }



            if (client.velX == 0 && client.velY == 0 && client.velZ == 0)
                return;
            //System.out.println(client.name+" velocity: "+vel.toString()+" onGround:"+client.onGround+" ajc"+autojumpcooldown);
            client.setposto(client.getPosition().add(client.velX, client.velY, client.velZ));
        }

        public void reset() {
            client.velX = 0;
            client.velY = 0;
            client.velZ = 0;
        }
    }

    public enum State {
        waitForPosPacket, waitForChunk, ready
    }
}
